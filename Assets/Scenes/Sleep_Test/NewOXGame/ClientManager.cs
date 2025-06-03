using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ClientManager : MonoBehaviourPunCallbacks
{
    public GameObject clientPrefab; // EEG 시각화를 위한 프리팹

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 마스터 서버에 연결됨, 서버 방 입장 시도...");
        PhotonNetwork.JoinRoom("EEGServerRoom");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("서버 방 입장 성공");

        // 클라이언트가 입장 시 자신의 프리팹을 생성
        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate(clientPrefab.name, spawnPos, Quaternion.identity);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"서버 방 입장 실패: {message}");
    }
}
