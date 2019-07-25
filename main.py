import numpy as np
import imgaug as ia
from imgaug.augmentables.batches import UnnormalizedBatch
from imgaug import augmenters as iaa
import imageio , os , glob , sys , random

def generateImage(loadPath,savePath):
 
 fileName = glob.glob(loadPath)
 if os.path.exists(savePath) == False:
  os.mkdir(savePath)
 images = []
 for name in fileName:
  image = imageio.imread(name)
  images.append(image)

 batches = [UnnormalizedBatch(images=images)]
 mode = random.randint(0,100)
 aug = iaa.Sequential()
 mode = mode % 3
 if mode == 0:
  size = random.uniform(0.01,0.02) 
  density = random.uniform(0.15,0.3) 
  aug.add(iaa.CoarseSalt((0.1,density), size_percent=(0.008,size)))
 elif mode == 1:
  col = random.randint(3,6)
  row = random.randint(3,6)
  replace = random.uniform(0.4,0.5) 
  aug.add(iaa.PiecewiseAffine(scale=(0.02), nb_cols=(2,col), nb_rows=(2,row)))
  aug.add(iaa.Superpixels(p_replace=(0.3,replace), n_segments=100) )	
 batches_aug = list(aug.augment_batches(batches, background=False))
 i = 0
 for name in fileName:
  imageio.imwrite(os.path.join(savePath, os.path.basename(name)), np.uint8(batches_aug[0].images_aug[i]))
  i = i + 1
 print("123")
  
  
if __name__ == '__main__':
 generateImage(sys.argv[1],sys.argv[2])