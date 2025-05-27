using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.IO;
using System;

public class EEGReceiver : MonoBehaviour
{
    TcpListener listener;
    TcpClient client;
    NetworkStream stream;
    StreamReader reader;

    public int attention;
    public int meditation;
    public int blink;

    void Start()
    {
        listener = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 5005);
        listener.Start();
        listener.BeginAcceptTcpClient(AcceptCallback, null);
        Debug.Log("EEG 수신 대기 중...");
    }

    void AcceptCallback(IAsyncResult ar)
    {
        client = listener.EndAcceptTcpClient(ar);
        stream = client.GetStream();
        reader = new StreamReader(stream, Encoding.UTF8);
        Debug.Log("EEG 연결됨");
    }

    void Update()
    {
        if (client != null && stream.DataAvailable)
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

                    Debug.Log($"️ Blink: {blink},  Attention: {attention}, Meditation: {meditation}");

                }
                catch (Exception ex)
                {
                    Debug.LogWarning("JSON 파싱 오류: " + ex.Message);
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
    }
}
