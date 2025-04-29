using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientHandler : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;

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
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            if (bytesRead > 0)
            {
                string received = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                if (float.TryParse(received, out float receivedFloat))
                {
                    Debug.Log($"[서버] 받은 float 값: {receivedFloat}");
                    // TODO: 받은 값을 Unity 오브젝트나 서버 데이터에 적용할 수 있음
                }
                else
                {
                    Debug.LogWarning("[서버] 수신 데이터 변환 실패: " + received);
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
