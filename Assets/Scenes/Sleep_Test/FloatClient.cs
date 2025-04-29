using System.Collections;
using System.Net.Sockets;
using UnityEngine;

public class FloatClient : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    public GameObject Gamma_A;

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
            if (Gamma_A != null)
            {
                float value = Gamma_A.GetComponent<InletOutlet2>().EEGpow;
                SendFloat(value);
            }
        }
    }

    void SendFloat(float value)
    {
        if (stream != null)
        {
            string msg = value.ToString();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
            stream.Write(data, 0, data.Length);
            Debug.Log($"[클라이언트] 보낸 float 값: {value}");
        }
    }

    void OnApplicationQuit()
    {
        if (stream != null) stream.Close();
        if (client != null) client.Close();
    }
}
