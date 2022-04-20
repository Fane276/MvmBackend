import uvicorn
from fastapi import FastAPI, Request, File, UploadFile, HTTPException
from starlette.requests import Request
from fastapi.templating import Jinja2Templates
from fastapi.middleware.cors import CORSMiddleware
import numpy as np
import io
import cv2
import pytesseract

from documentScann import ImagePreProcessing, PostProcessing


app = FastAPI()
templates = Jinja2Templates(directory="templates")
origins = [
    "http://localhost.tiangolo.com",
    "https://localhost.tiangolo.com",
    "http://localhost",
    "http://localhost:8080",
    "http://localhost:3000",
]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

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
async def extract_text(file: UploadFile):
    label = ""
    contents = await file.read()
    image_stream = io.BytesIO(contents)
    image_stream.seek(0)
    file_bytes = np.asarray(bytearray(image_stream.read()), dtype=np.uint8)
    frame = cv2.imdecode(file_bytes, cv2.IMREAD_COLOR)
    label =  read_img(frame)

    if(label['FromDate'] == None or label['ToDate'] == None):
        raise HTTPException(status_code=500, detail="Data could not be parsed")

    # return {"label": label}
   
    return label
    
if __name__ == "__main__":
    uvicorn.run("app:app", host="127.0.0.1", port=8000, reload=True)