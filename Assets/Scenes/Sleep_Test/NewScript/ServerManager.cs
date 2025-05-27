using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ServerManager : MonoBehaviour
{
    public GameObject clientPrefab;        // UI용 프리팹 (예: 600x300)
    public Transform parentTransform;      // UI 오브젝트를 붙일 부모 (예: Canvas 하위 빈 오브젝트)

    private TcpListener server;
    private UnityMainThreadDispatcher dispatcher;

    private int clientIndex = 0;

    // 6개 위치 슬롯 (1920x1080 기준)
    private Vector2[] slotPositions = new Vector2[]
    {
        new Vector2(-480, 480),
        new Vector2(480, 480),
        new Vector2(-480, 0),
        new Vector2(480, 0),
        new Vector2(-480, -480),
        new Vector2(480, -480)
    };

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
                GameObject clientObj = Instantiate(clientPrefab, parentTransform);

                // 위치 지정
                RectTransform rt = clientObj.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchoredPosition = slotPositions[clientIndex];
                }

                clientIndex++;

                // 클라이언트 네트워크 핸들링 초기화
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
