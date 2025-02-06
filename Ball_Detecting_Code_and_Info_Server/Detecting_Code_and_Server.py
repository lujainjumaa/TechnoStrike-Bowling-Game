import cv2
import numpy as np
from flask import Flask, jsonify
import threading
import time  
import logging

app = Flask(__name__)

output = {"VX": None, "VZ": None, "Xpos": None, "BShot": False}

@app.route('/', methods=['GET'])
def send_data():
    global output
    data = {
        'VX': output['VX'],
        'VZ': output['VZ'],
        'Xpos': output['Xpos'],
        'BShot': output['BShot']
    }
    return jsonify(data), 200, {'Content-Type': 'application/json'}

# Route to stream video frames for real-time monitoring

# @app.route('/video_feed')
# def video_feed():
#     return Response(generate_frames(),
#                     mimetype='multipart/x-mixed-replace; boundary=frame')

# Function to process video and detect orange object
def process_video():
    global output
    
    framesInfo = []

    while True:  

        framesInfo.clear()  # Clear frames information for each new cycle

        framesInfo, crops = get_ball_frames()

        if len(framesInfo) > 1:
            Vx, Vz, Xpos = calculate_velocity(framesInfo[0], framesInfo[-1], crops)
            output['VX'] = Vx
            output['VZ'] = Vz
            output['Xpos'] = Xpos
            output['BShot'] = True
            print(Vx, "  ", Vz, "  ", Xpos)
        else:
            print("Not enough data points to calculate velocity.")

        # Wait for a few seconds before resetting the output for the next detection cycle
        time.sleep(3)

        output['VX'] = None
        output['VZ'] = None
        output['Xpos'] = None
        output['BShot'] = False

        print("Ball is out of frame. Re-waiting for the ball...")

    cap.release()
    cv2.destroyAllWindows()

# FUNCTIONS:

def findContours(cropped_frame):
    hsv = cv2.cvtColor(cropped_frame, cv2.COLOR_BGR2HSV)

    lower_orange = np.array([6, 130, 130])
    upper_orange = np.array([15, 255, 255])

    mask = cv2.inRange(hsv, lower_orange, upper_orange)

    kernel = np.ones((5, 5), np.uint8)
    mask = cv2.morphologyEx(mask, cv2.MORPH_CLOSE, kernel)
    mask = cv2.morphologyEx(mask, cv2.MORPH_OPEN, kernel)

    contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    return contours

def calculate_velocity(firstFrame, lastFrame,crops):
    X1, Z1, t1 = firstFrame
    X2, Z2, t2 = lastFrame
    x, y, width, hight = crops
    X1_mapped = map_range(X1, 0, width, -100 * 1.7, 100 * 1.7)
    Z1_mapped = map_range(Z1, 0, hight, -100, 100)
    X2_mapped = map_range(X2, 0, width, -100 * 1.7, 100 * 1.7)
    Z2_mapped = map_range(Z2, 0, hight, -100, 100)

    delta_x = X2_mapped - X1_mapped
    delta_z = Z2_mapped - Z1_mapped
    delta_t = t2 - t1

    velocity_x = delta_x / delta_t
    velocity_z = delta_z / delta_t

    return velocity_x / 2, velocity_z / 2, Z2_mapped

def returnInfo(contours):
    contour = max(contours, key=cv2.contourArea)
    (X, Z), _ = cv2.minEnclosingCircle(contour)
    current_time = time.time()
    return X, Z, current_time

def crop_frame(frame ,crops):
    (x, y, width, height) = crops
    return frame[y:y + height, x:x + width]

def map_range(value, old_min, old_max, new_min, new_max):
    if old_max - old_min == 0:
        raise ValueError("The input range cannot have zero length.")
    value = max(old_min, min(value, old_max))
    new_value = (value - old_min) / (old_max - old_min) * (new_max - new_min) + new_min
    return new_value

#show frames from the camera connect to the raspberry pi

# def generate_frames():
#     while True:
#         cropped_frame = crop_frame(frame,get_crops(get_cap()))
#         ret, buffer = cv2.imencode('.jpg', cropped_frame)
#         cropped_frame = buffer.tobytes()
#         yield (b'--frame\r\n'
#                b'Content-Type: image/jpeg\r\n\r\n' + cropped_frame + b'\r\n')

def crop_rectangle(image):
    
    # Convert the image to grayscale for white rectangle detection
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)

    _, white_thresh = cv2.threshold(gray, 120, 255, cv2.THRESH_BINARY)
    cv2.imshow('white_thresh',white_thresh)

    white_contours, _ = cv2.findContours(white_thresh, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

    # Check if any white contours were found
    if white_contours:

        largest_white_contour = max(white_contours, key=cv2.contourArea)


        crops = cv2.boundingRect(largest_white_contour)

        # Crop the image to the white rectangle (color image)
        cropped_to_white = crop_frame(image,crops)

        cv2.imshow('cropped_to_white',cropped_to_white)

        return cropped_to_white, crops
    
def get_cap():
    cap = cv2.VideoCapture(0, cv2.CAP_DSHOW)
    cap.set(cv2.CAP_PROP_FRAME_WIDTH, 1280)  
    cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 720) 

    if not cap.isOpened():
        print("Error: Unable to open video stream")
        exit
    return cap

def get_crops(cap):
    while True:
        ret, frame = cap.read()
        if(not ret): break
        
        frame, (x, y, width, height) = crop_rectangle(frame)
        if(height > 100 and width > 100):
            print("width=",width)
            print("height=",height)
            print("x=",x)
            print("y=",y)
            return (x, y, width, height)
        
def get_ball_frames():
    Bfound = False
    BOutOfFrame = 0

    framesList = []

    cap = get_cap()

    crops = get_crops(cap)

    print("Waiting for the ball to come into frame...")

    while not output['BShot']:
        ret, frame = cap.read()
        
        if not ret:
            print("Failed to grab frame")
            break

        # cv2.imshow('frame', frame)

        cropped_frame = crop_frame(frame,crops)

        # cv2.imshow('after_crop',cropped_frame)
        
        contours = findContours(cropped_frame)
        
        if contours:
            Bfound = True
            X, Z, current_time = returnInfo(contours)
            framesList.append((X, Z, current_time))

            cv2.imshow('croppedFrame', cropped_frame)

        else:
            if Bfound:
                if BOutOfFrame > 4:
                    break
                else:
                    BOutOfFrame += 1

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
    
    return framesList,crops

if __name__ == '__main__':
    video_thread = threading.Thread(target=process_video)
    video_thread.daemon = True
    video_thread.start()

    log = logging.getLogger('werkzeug')
    log.setLevel(logging.ERROR)
    
    app.run(host='0.0.0.0', port=5000)
