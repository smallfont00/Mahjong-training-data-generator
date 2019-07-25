import numpy as np
import imgaug as ia
from imgaug.augmentables.batches import UnnormalizedBatch
from imgaug import augmenters as iaa
import imageio
import os
import glob
import sys
import random

def generateImage(loadPath, savePath):

    fileName = glob.glob(loadPath)

    if os.path.exists(savePath) == False:
        os.makedirs(savePath)

    images = list(map(lambda name: imageio.imread(name), fileName))

    batches = UnnormalizedBatch(images=images)
    mode = random.randint(0, 100) % 3
    aug = iaa.Sequential()

    if mode == 0:
        size = random.uniform(0.01, 0.02)
        density = random.uniform(0.15, 0.3)
        aug.add(iaa.CoarseSalt((0.1, density), size_percent=(0.008, size)))
    elif mode == 1:
        col = random.randint(3, 6)
        row = random.randint(3, 6)
        replace = random.uniform(0.4, 0.5)
        aug.add(iaa.PiecewiseAffine(scale=(0.02), nb_cols=(2, col), nb_rows=(2, row)))
        aug.add(iaa.Superpixels(p_replace=(0.3, replace), n_segments=100))

    batches_aug = list(aug.augment_batches(batches, background=False))[0]

    for i, name in enumerate(fileName):
        imageio.imwrite(os.path.join(savePath, os.path.basename(name)), np.uint8(batches_aug.images_aug[i]))


if __name__ == '__main__':
    generateImage(sys.argv[1], sys.argv[2])
