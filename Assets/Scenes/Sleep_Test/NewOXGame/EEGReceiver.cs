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

    void Start()
    {
        // 2초 지연 후 연결 시도
        Invoke(nameof(ConnectToEEGServer), 2f);
    }

    void ConnectToEEGServer()
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
