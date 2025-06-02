using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ClientManager : MonoBehaviourPunCallbacks
{
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
        // 방 입장 후 EEGReceiver 스크립트가 데이터 전송 시작
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"서버 방 입장 실패: {message}");
    }
}
