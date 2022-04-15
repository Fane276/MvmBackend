
import cv2
import numpy as np
import matplotlib.pyplot as plt
from sklearn.preprocessing import scale

file =  r'Data/IMG_3524.PNG'

im1 = cv2.imread(file, 0)
im = cv2.imread(file)

scale_percent = 100 # percent of original size
width = int(im.shape[1] * scale_percent / 100)
height = int(im.shape[0] * scale_percent / 100)
dim = (width, height)
  
# resize image
im = cv2.resize(im, dim, interpolation = cv2.INTER_AREA)

ret,thresh_value = cv2.threshold(im1,180,255,cv2.THRESH_BINARY_INV)

kernel = np.ones((5,5),np.uint8)
dilated_value = cv2.dilate(thresh_value,kernel,iterations = 1)

contours, hierarchy = cv2.findContours(dilated_value,cv2.RETR_TREE,cv2.CHAIN_APPROX_SIMPLE)
cordinates = []
for cnt in contours:
    x,y,w,h = cv2.boundingRect(cnt)
    cordinates.append((x,y,w,h))
    #bounding the images
    if y< 50:
        
        cv2.rectangle(im,(x,y),(x+w,y+h),(0,0,255),1)
        
plt.imshow(im)
cv2.imshow("img",im)
cv2.waitKey(0)
# cv2.namedWindow('detecttable', cv2.WINDOW_NORMAL)
# cv2.imwrite('detecttable.jpg',im)