using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.IO;
using System;
using Photon.Pun;

public class EEGReceiver : MonoBehaviourPun
{
    TcpClient client;
    NetworkStream stream;
    StreamReader reader;

    public int attention;
    public int meditation;
    public int blink;
    public int delta, theta, lowAlpha, highAlpha;
    public int lowBeta, highBeta, lowGamma, highGamma;

    private float sendInterval = 0.1f; // 0.1초마다 서버로 전송
    private float timeSinceLastSend = 0f;

    void Start()
    {
        try
        {
            client = new TcpClient("127.0.0.1", 5005);
            stream = client.GetStream();
            reader = new StreamReader(stream, Encoding.UTF8);
            Debug.Log("파이썬 EEG 서버에 연결됨");
        }
        catch (Exception ex)
        {
            Debug.LogError("연결 실패: " + ex.Message);
        }
    }

    void Update()
    {
        // TCP로부터 데이터 수신
        if (client != null && stream != null && stream.DataAvailable)
        {
            string line = reader.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                try
                {
                    EEGData data = JsonUtility.FromJson<EEGData>(line);

                    attention = data.attention;
                    meditation = data.meditation;
                    blink = data.blink;
                    delta = data.delta;
                    theta = data.theta;
                    lowAlpha = data.lowAlpha;
                    highAlpha = data.highAlpha;
                    lowBeta = data.lowBeta;
                    highBeta = data.highBeta;
                    lowGamma = data.lowGamma;
                    highGamma = data.highGamma;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("JSON 파싱 실패: " + ex.Message);
                }
            }
        }

        // 일정 주기마다 PUN RPC로 서버(마스터클라이언트)에 EEG 데이터 전송
        timeSinceLastSend += Time.deltaTime;
        if (timeSinceLastSend >= sendInterval)
        {
            SendEEGDataToServer();
            timeSinceLastSend = 0f;
        }
    }

    private void SendEEGDataToServer()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("ReceiveEEGData", RpcTarget.MasterClient,
                attention, meditation, blink,
                delta, theta, lowAlpha, highAlpha,
                lowBeta, highBeta, lowGamma, highGamma);
        }
    }

    [Serializable]
    public class EEGData
    {
        public int attention;
        public int meditation;
        public int blink;
        public int delta, theta, lowAlpha, highAlpha;
        public int lowBeta, highBeta, lowGamma, highGamma;
    }
}
