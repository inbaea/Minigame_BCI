using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.IO;
using System;
using Photon.Pun;
using System.Collections;

public class EEGReceiver : MonoBehaviourPun
{
    TcpClient client;
    NetworkStream stream;
    StreamReader reader;

    public int attention, meditation, blink;
    public int delta, theta, lowAlpha, highAlpha;
    public int lowBeta, highBeta, lowGamma, highGamma;

    private float sendInterval = 0.1f;
    private float timeSinceLastSend = 0f;

    private bool isConnected = false;

    IEnumerator ConnectWithDelay()
    {
        yield return new WaitForSeconds(2f);

        try
        {
            client = new TcpClient("127.0.0.1", 5005);
            stream = client.GetStream();
            reader = new StreamReader(stream, Encoding.UTF8);
            Debug.Log("파이썬 EEG 서버에 연결됨");
            isConnected = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("연결 실패: " + ex.Message);
        }
    }

    void Start()
    {
        StartCoroutine(ConnectWithDelay());
    }

    void Update()
    {
        if (!isConnected) return;

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

        timeSinceLastSend += Time.deltaTime;
        if (timeSinceLastSend >= sendInterval)
        {
            SendEEGDataToServer();
            timeSinceLastSend = 0f;
        }
    }

    private void SendEEGDataToServer()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            // 자신의 photonView.ViewID를 서버에 전달해서 서버가 정확한 프리팹에 전달할 수 있게 한다.
            photonView.RPC("RelayEEGData", RpcTarget.MasterClient,
                photonView.ViewID,
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
