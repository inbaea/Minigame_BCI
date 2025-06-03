using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("포톤 네트워크 연결 시도...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 연결됨, 방 생성 시도...");
        PhotonNetwork.JoinOrCreateRoom("EEGServerRoom", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 성공");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트입니다.");
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        Debug.Log($"플레이어 입장: {newPlayer.NickName} (ActorNumber: {newPlayer.ActorNumber})");
    }
}
