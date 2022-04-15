import cv2
import numpy as np
import pytesseract

# aling the image 
def rectify(h):
  h=h.reshape((4,2))
  hnew = np.zeros((4,2), dtype= np.float32)

  add = h.sum(1)
  hnew[0] = h[np.argmin(add)]
  hnew[2] = h[np.argmax(add)]

  diff = np.diff(h, axis = 1)
  hnew[1] = h[np.argmin(diff)]
  hnew[3] = h[np.argmax(diff)]

  return hnew

def resize_image(image, width, height):
  image = cv2.resize(image, (width,height))
  return image;

def gray_scale(image):
  image = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
  return image;

def canny_edge_detection(image):
  blurred_image= cv2.GaussianBlur(image, (5,5), 0)
  edges = cv2.Canny(blurred_image, 0, 50)
  return edges

def find_contours(image):
  (contours, _) = cv2.findContours(image, cv2.RETR_LIST, cv2.CHAIN_APPROX_NONE)
  contours = sorted(contours, key=cv2.contourArea, reverse=True)
  for c in contours:
    p=cv2.arcLength(c, True)
    approx = cv2.approxPolyDP(c, 0.02 * p, True)

    if len(approx) == 4:
      target = approx
      break
  return target

def draw_contours(orig_image, image, target):
  approx = rectify(target)
  pts2 = np.float32([[0,0], [800,0], [800,800], [0,800]])

  M = cv2.getPerspectiveTransform(approx, pts2)
  result = cv2.warpPerspective(image, M, (800,800))

  cv2.drawContours(image, [target], -1, (0,255,0), 2)
  result = cv2.cvtColor(result, cv2.COLOR_BGR2GRAY)
  return result

# image = cv2.imread("Data/IMG_3216.JPG") # poza asigurare
# image = cv2.imread("Data/IMG_3513.jpg") # poza buletin
image = cv2.imread("Data/buletin 2.JPG") # poza buletin 2
# image = cv2.imread("Data/IMG_3524.PNG") # ss asigurare
# image = cv2.imread("Data/IMG_4425.JPG") # bon
iamge = resize_image(image, 1500, 800)
orig = image.copy()
cv2.imshow("Original", orig)

gray = gray_scale(image)
cv2.imshow("Gray", gray)

edges = canny_edge_detection(gray)
cv2.imshow("Edges", edges)

target = find_contours(edges)
output= draw_contours(orig, image, target)
text = pytesseract.image_to_string(output, lang='ron', config='--psm 1 ')

print("Text Detected: \n" + text)
cv2.imshow("Output", output)

cv2.waitKey(0)