using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SendData : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;

    public GameObject EEGManager_Gamma;
    public GameObject EEGManager_Beta;
    public GameObject EEGManager_Alpha;
    public GameObject EEGManager_Theta;
    public GameObject EEGReceiver;

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
        public List<ClientDataStore.EEG_Store> gamma;
        public List<ClientDataStore.EEG_Store> beta;
        public List<ClientDataStore.EEG_Store> alpha;
        public List<ClientDataStore.EEG_Store> theta;
        public EEGData metaData;
    }

    void Start()
    {
        StartCoroutine(ConnectAfterDelay());
    }

    IEnumerator ConnectAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        client = new TcpClient("172.18.158.215", 7777);
        stream = client.GetStream();
        Debug.Log("서버 연결 성공!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendAllData(); // 스페이스 누르면 전체 데이터 전송
        }
    }

    void SendAllData()
    {
        if (stream == null) return;

        EEG_Packet packet = new EEG_Packet
        {
            gamma = EEGManager_Gamma.GetComponent<ClientDataStore>().storedData,
            beta = EEGManager_Beta.GetComponent<ClientDataStore>().storedData,
            alpha = EEGManager_Alpha.GetComponent<ClientDataStore>().storedData,
            theta = EEGManager_Theta.GetComponent<ClientDataStore>().storedData,
            metaData = new EEGData
            {
                attention = EEGReceiver.GetComponent<EEGReceiver>().attention,
                meditation = EEGReceiver.GetComponent<EEGReceiver>().meditation,
                blink = EEGReceiver.GetComponent<EEGReceiver>().blink
            }
        };

        string json = JsonUtility.ToJson(packet);
        byte[] data = Encoding.UTF8.GetBytes(json);
        stream.Write(data, 0, data.Length);

        Debug.Log("EEG 데이터 전송 완료!");
    }

    void OnApplicationQuit()
    {
        if (stream != null) stream.Close();
        if (client != null) client.Close();
    }
}
