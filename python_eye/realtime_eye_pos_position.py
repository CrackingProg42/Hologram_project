from imutils.video import VideoStream
from imutils import face_utils
import imutils
import socket
import dlib
import cv2
import numpy as np

UDP_IP = "127.0.0.1"
UDP_PORT = 5065

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

def get_eyes(eyes):
    return ((np.mean([i[0] for i in eyes]), np.mean([i[1] for i in eyes])))

detector = dlib.get_frontal_face_detector()
predictor = dlib.shape_predictor("shape_predictor_68_face_landmarks.dat")
vs = VideoStream(0).start()

while True:
    frame = vs.read()
    frame = imutils.resize(frame, width=800)
    frame = cv2.flip( frame, 1)
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    rects = detector(gray, 0)
    for rect in rects:
        shape = predictor(gray, rect)
        shape = face_utils.shape_to_np(shape)
        eyes = get_eyes([shape[i] for i in range(36, 48)])
        res = ",".join(str(i) for i in [((int)(eyes[0]) - 400) / 400, ((int)(eyes[1]) - 400) / 400])
    sock.sendto(res.encode(), (UDP_IP, UDP_PORT))
    print(res)
    key = cv2.waitKey(1) & 0xFF
    if key == ord("q"):
        break
cv2.destroyAllWindows()
vs.stop()
