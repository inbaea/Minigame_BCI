using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class FloatServer : MonoBehaviour
{
    TcpListener server;
    Thread serverThread;

    void Start()
    {
        serverThread = new Thread(StartServer);
        serverThread.Start();
    }

    void StartServer()
    {
        server = new TcpListener(IPAddress.Any, 7777);
        server.Start();
        Debug.Log("서버 시작됨.");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (client.Connected)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                string msg = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                if (float.TryParse(msg, out float receivedValue))
                {
                    Debug.Log($"받은 float 값: {receivedValue}");
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        server.Stop();
        serverThread.Abort();
    }
}
