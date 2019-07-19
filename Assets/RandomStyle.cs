using UnityEngine;
using Random = UnityEngine.Random;

public class RandomStyle : MonoBehaviour
{
    // Start is called before the first frame update
    public bool filped = false;
    public bool visiable = true;
    private Vector3 m_OriginalLocalEulerAngles;
    private Collider m_Collider;
    private Renderer m_Renderer;
    public string type { get; internal set; }
    public static string[] resourcesPath = new[] { "imgaug1/", "imgaug2/", "imgaug3/", "imgaug4/", "imgaug5/", "imgau6/" };

    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_Collider = GetComponent<Collider>();
        m_OriginalLocalEulerAngles = transform.localEulerAngles;
    }

    private void FixedUpdate()
    {
        m_Renderer.enabled = visiable;
        m_Collider.enabled = visiable;
        transform.localEulerAngles = m_OriginalLocalEulerAngles + (filped ? new Vector3(0, 180, 0) : Vector3.zero);
    }

    private static void ChangeAllStyleRandomly(bool flipEnable = false, bool visiableEnable = false)
    {
        foreach (RandomStyle mjStyle in FindObjectsOfType<RandomStyle>())
        {
            mjStyle.ChangeStyleRandomly(flipEnable, visiableEnable);
        }
    }

    private string ToPhotoFormat(int number)
    {
        return $"p({number})";
    }
    public void ChangeStyleRandomly(bool flipEnable = true, bool visibleEnable = true)
    {
        if (flipEnable) filped = Random.Range(0, 2) == 1;
        if (visibleEnable) visiable = Random.Range(0, 2) == 1;
        var mat = GetComponent<Renderer>().materials[0];

        int typeNumber = resourcesPath.Length;
        var randomType = Random.Range(0, typeNumber);
        var randomCard = Random.Range(0, 42);
        var path = resourcesPath[randomType] + ToPhotoFormat(randomCard);

        var nextTexture = Resources.Load<Texture2D>(path);
        for (int i = (randomType + 1) % typeNumber, j = 0; nextTexture == null; i = (i + 1) % typeNumber, j++)
        {
            path = resourcesPath[i] + ToPhotoFormat(randomCard);
            nextTexture = Resources.Load<Texture2D>(path);
        }
        mat.mainTexture = nextTexture;
        type = randomCard.ToString();
    }

    public YoloData GetYoloData(Camera cam)
    {
        var boxPoint = GetComponent<Renderer>().bounds.center;
        var boxSize = GetComponent<Renderer>().bounds.extents;

        var boxUp = Vector3.up * boxSize.y;
        var boxRight = Vector3.right * boxSize.x;
        var boxForward = Vector3.forward * boxSize.z;

        Vector3[] box = new Vector3[3] { boxUp, boxRight, boxForward };
        float left = Screen.width, right = 0, up = 0, down = Screen.height;
        for (int i = 0; i < (1 << box.Length); i++)
        {
            var tmp = boxPoint;
            for (int j = 0; j < box.Length; j++)
            {
                tmp += ((i & (1 << j)) != 0) ? box[j] : (-box[j]);
            }
            tmp = cam.WorldToScreenPoint(tmp);
            left = Mathf.Min(left, tmp.x);
            right = Mathf.Max(right, tmp.x);
            up = Mathf.Max(up, tmp.y);
            down = Mathf.Min(down, tmp.y);
        }
        return new YoloData(new Rect(left, Screen.height - up, right - left, up - down), type);
    }
}