using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject eegDisplayPrefab;

    void Start()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 마스터 서버에 연결됨, 방 생성 시도...");
        PhotonNetwork.CreateRoom("EEGServerRoom");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 성공");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"방 생성 실패: {message}");
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        SpawnClientPrefab(newPlayer);
    }

    void SpawnClientPrefab(Player player)
    {
        if (eegDisplayPrefab != null)
        {
            GameObject obj = Instantiate(eegDisplayPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("클라이언트 EEG 표시용 프리팹 생성");
        }
        else
        {
            Debug.LogError("eegDisplayPrefab이 할당되지 않았습니다!");
        }
    }
}
