import uvicorn
from fastapi import FastAPI, Request, File, UploadFile
from starlette.requests import Request
from fastapi.templating import Jinja2Templates
import numpy as np
import io
import cv2
import pytesseract

from documentScann import ImagePreProcessing, PostProcessing


app = FastAPI()
templates = Jinja2Templates(directory="templates")

@app.get("/")
def home(request: Request):
    return templates.TemplateResponse("index.html", {"request": request})


def read_img(img):
    img = ImagePreProcessing(img)
    text = pytesseract.image_to_string(img, lang='ron', config='--psm 1')
    object = PostProcessing(text)
    return(object)

#  , file: bytes = File(...)

@app.post("/extract_text") 
async def extract_text(request: Request):
    label = ""
    if request.method == "POST":
        form = await request.form()
        # file = form["upload_file"].file
        contents = await form["upload_file"].read()
        image_stream = io.BytesIO(contents)
        image_stream.seek(0)
        file_bytes = np.asarray(bytearray(image_stream.read()), dtype=np.uint8)
        frame = cv2.imdecode(file_bytes, cv2.IMREAD_COLOR)
        label =  read_img(frame)
       
        # return {"label": label}
   
    return templates.TemplateResponse("index.html", {"request": request, "label": label})
    
if __name__ == "__main__":
    uvicorn.run("app:app", host="127.0.0.1", port=8000, reload=True)