import numpy as np
import imgaug as ia
import imageio
import cv2
import os
from imgaug.augmentables.batches import UnnormalizedBatch
from imgaug import augmenters as iaa
import sys
import random
import time

def generateImage(loadPath,savePath):
 
 fileName = os.listdir(loadPath)
 counts = len(fileName)
 temp = []
 for name in fileName:
  if name[-3:] == 'jpg':
   image = imageio.imread(loadPath + '\\' + name)
   temp.append(image)
   
 images = [np.copy(temp)]
 batches = [UnnormalizedBatch(images=images[0])]
 mode = random.randint(0,100)
 aug = iaa.Sequential()
 mode = mode % 3
 #mode = 2
 if mode == 0:
   size = random.uniform(0.01,0.02) 
   density = random.uniform(0.15,0.3) 
   aug.add(iaa.CoarseSalt((0.1,density), size_percent=(0.008,size)))
 else:
   if mode == 1:
    col = random.randint(3,6)
    row = random.randint(3,6)
    replace = random.uniform(0.4,0.5) 
    aug.add(iaa.PiecewiseAffine(scale=(0.02), nb_cols=(2,col), nb_rows=(2,row)))
    aug.add(iaa.Superpixels(p_replace=(0.3,replace), n_segments=100) )	
 batches_aug = list(aug.augment_batches(batches, background=False))
 i = 0
 for name in fileName:
  if name[-3:] == 'jpg':
   cv2.imwrite(os.path.join(savePath, name), batches_aug[0].images_aug[i])
   i = i + 1
 #byte = bytes(cv2.imencode('.jpg', batches_aug[0].images_aug[0])[1])
 #byteStr = ""
 #for i in range(len(byte)):
  #if i != 0:
   #byteStr = byteStr+","
  #byteStr = byteStr + str(byte[i])
 #print(byteStr)
  
  
if __name__ == '__main__':
 generateImage(sys.argv[1],sys.argv[2])