using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientHandler : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;

    [System.Serializable]
    public class EEG_Store
    {
        public string eegType;
        public float eegPower;
        public string topic;
        public string questionText;
        public bool currCorrected;
        public string level;
    }

    [System.Serializable]
    public class EEGData
    {
        public int attention;
        public int meditation;
        public int blink;
    }

    [System.Serializable]
    public class EEG_Packet
    {
        public List<EEG_Store> gamma;
        public List<EEG_Store> beta;
        public List<EEG_Store> alpha;
        public List<EEG_Store> theta;
        public EEGData metaData;
    }

    public void Init(TcpClient tcpClient)
    {
        client = tcpClient;
        stream = client.GetStream();
        Debug.Log("클라이언트 초기화 완료");
    }

    void Update()
    {
        if (client != null && client.Connected && stream.DataAvailable)
        {
            byte[] buffer = new byte[8192]; // 더 큰 버퍼로 JSON 전체 받기
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            if (bytesRead > 0)
            {
                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("[서버] 받은 원시 JSON: " + received);

                try
                {
                    EEG_Packet packet = JsonUtility.FromJson<EEG_Packet>(received);

                    // alpha 출력 예시
                    if (packet.alpha != null)
                    {
                        foreach (var entry in packet.alpha)
                        {
                            Debug.Log($"[alpha] EEGPower: {entry.eegPower}, Question: {entry.questionText}, Blink: {packet.metaData.blink}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("alpha 리스트가 비어 있거나 null임");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON 파싱 실패: " + e.Message);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (stream != null) stream.Close();
        if (client != null) client.Close();
    }
}
