import cv2
import numpy as np
import pytesseract
import datefinder
import re

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
  blurred_image= cv2.GaussianBlur(image, (7,7), 0)
  # blurred_image= cv2.adaptiveThreshold(image, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY, 85, 15);
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
  pts2 = np.float32([[0,0], [1000,0], [1000,800], [0,800]])

  M = cv2.getPerspectiveTransform(approx, pts2)
  result = cv2.warpPerspective(image, M, (1000,800))

  cv2.drawContours(orig_image, [target], -1, (0,255,0), 2)
  result = cv2.cvtColor(result, cv2.COLOR_BGR2GRAY)
  return result

def ImagePreProcessing(image):
  image = resize_image(image, 900, 1600)
  # image = resize_image(image, 1400, 900)
  orig = image.copy()
  # cv2.imshow("Original", orig)

  gray = gray_scale(image)
  # cv2.imshow("Gray", gray)

  edges = canny_edge_detection(gray)

  target = find_contours(edges)
  output= draw_contours(orig, image, target)
  # text = pytesseract.image_to_string(output, lang='ron')

  return output

def findDateInText(text):
  # a regular expression pattern to match dates
  pattern = "\d{2}[./-]\d{2}[./-]\d{4}"

  
  # find all the strings that match the pattern
  dates = re.findall(pattern, text)

  for date in dates:
      return date

def PostProcessing(text):
  text = text.split('\n')
  textPerLines =[]
  for line in text:
    if not line.isspace():
        textPerLines.append(line)
  obj={"FirstName":'', "LastName":'',"FromDate": '', 'ToDate': ''}
  # obj["FromDate"] = textPerLines[5]
  obj["LastName"] = ' '.join(textPerLines[2].split(" ")[1:])
  obj["FirstName"] = ' '.join(textPerLines[3].split(" ")[1:])
  obj["FromDate"] = findDateInText(textPerLines[5])
  obj["ToDate"] = findDateInText(textPerLines[6])
  return obj


# image = cv2.imread("Data/IMG_3216.JPG") # poza talon 1
# image = cv2.imread("Data/IMG_4444.JPG") # poza talon 2
# image = cv2.imread("Data/IMG_4445.JPG") # poza talon 3
# image = cv2.imread("Data/IMG_4446.JPG") # poza talon 4
# image = cv2.imread("Data/IMG_4447.JPG") # poza talon 5
# image = cv2.imread("Data/IMG_3513.jpg") # poza buletin
# image = cv2.imread("Data/buletin 2.JPG") # poza buletin 2
# image = cv2.imread("Data/buletin moni 1.jpeg") # poza buletin 3
# image = cv2.imread("Data/buletin moni 2.jpeg") # poza buletin 4
# image = cv2.imread("Data/IMG_3524.PNG") # ss asigurare
# image = cv2.imread("Data/IMG_4425.JPG") # bon
# image = cv2.imread("Data/IMG_4448.JPG") # bon 2
# image = cv2.imread("Data/IMG_4453.JPG") # carnet

# # scale_percent = 60 # percent of original size
# # width = int(image.shape[1] * scale_percent / 100)
# # height = int(image.shape[0] * scale_percent / 100)
# # image = resize_image(image, width, height)

# image = resize_image(image, 900, 1600)
# # image = resize_image(image, 1400, 900)
# orig = image.copy()
# # cv2.imshow("Original", orig)

# gray = gray_scale(image)
# # cv2.imshow("Gray", gray)

# edges = canny_edge_detection(gray)
# cv2.imshow("Edges", edges)

# target = find_contours(edges)
# output= draw_contours(orig, image, target)
# text = pytesseract.image_to_string(output, lang='ron')

# print("Text Detected: \n" + text)
# cv2.imshow("Output", output)
# cv2.imshow("orig", orig)

# cv2.waitKey(0)

# output = ImagePreProcessing(image)
# text = pytesseract.image_to_string(output, lang='ron')

# print(text)

# obj = PostProcessing(text)
# print(obj)