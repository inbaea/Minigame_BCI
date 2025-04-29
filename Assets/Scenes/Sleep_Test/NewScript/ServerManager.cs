using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ServerManager : MonoBehaviour
{
    public GameObject clientPrefab;
    private TcpListener server;
    private UnityMainThreadDispatcher dispatcher;

    void Start()
    {
        dispatcher = UnityMainThreadDispatcher.Instance();

        server = new TcpListener(IPAddress.Any, 7777);
        server.Start();
        Debug.Log("서버가 실행 중입니다.");

        Thread acceptThread = new Thread(AcceptClients);
        acceptThread.Start();
    }

    void AcceptClients()
    {
        while (true)
        {
            TcpClient newClient = server.AcceptTcpClient();
            Debug.Log("새 클라이언트가 연결되었습니다!");

            dispatcher.Enqueue(() =>
            {
                GameObject clientObj = Instantiate(clientPrefab);
                ClientHandler handler = clientObj.GetComponent<ClientHandler>();
                handler.Init(newClient);
            });
        }
    }

    private void OnApplicationQuit()
    {
        server.Stop();
    }
}
