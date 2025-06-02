import socket
import json

# EEG 장치와 연결
HOST_EEG = '127.0.0.1'
PORT_EEG = 13854

eeg_sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
eeg_sock.connect((HOST_EEG, PORT_EEG))

# ThinkGear 설정
config = json.dumps({
    "enableRawOutput": False,
    "format": "Json"
})
eeg_sock.sendall(config.encode('utf-8'))

print("EEG 데이터 수신 중...")

# 유니티 클라이언트를 기다릴 서버 설정
HOST_SERVER = '127.0.0.1'
PORT_SERVER = 5005

server_sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_sock.bind((HOST_SERVER, PORT_SERVER))
server_sock.listen(1)
print(f"유니티 클라이언트 대기 중 (포트 {PORT_SERVER})...")

unity_conn, addr = server_sock.accept()
print(f"유니티 클라이언트 연결됨: {addr}")

# 데이터 중계 루프
while True:
    data = eeg_sock.recv(1024)
    if not data:
        break
    try:
        payload = json.loads(data.decode('utf-8'))
        result = {}

        if 'eSense' in payload:
            result['attention'] = payload['eSense']['attention']
            result['meditation'] = payload['eSense']['meditation']

        if 'blinkStrength' in payload:
            result['blink'] = payload['blinkStrength']

        if 'eegPower' in payload:
            result.update({
                'delta': payload['eegPower'].get('delta', 0),
                'theta': payload['eegPower'].get('theta', 0),
                'lowAlpha': payload['eegPower'].get('lowAlpha', 0),
                'highAlpha': payload['eegPower'].get('highAlpha', 0),
                'lowBeta': payload['eegPower'].get('lowBeta', 0),
                'highBeta': payload['eegPower'].get('highBeta', 0),
                'lowGamma': payload['eegPower'].get('lowGamma', 0),
                'highGamma': payload['eegPower'].get('highGamma', 0)
            })

        if result:
            message = json.dumps(result) + "\n"
            unity_conn.sendall(message.encode('utf-8'))
            print("전송됨:", message.strip())

    except json.JSONDecodeError:
        continue
