using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject eegDisplayPrefab; // 서버에서 생성할 EEG 표시용 프리팹 (PhotonView 포함)

    // 플레이어 ActorNumber → 서버 EEG 표시 프리팹 매핑
    private Dictionary<int, GameObject> playerEEGObjects = new Dictionary<int, GameObject>();

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

        Debug.Log($"플레이어 입장: {newPlayer.NickName} (ActorNumber: {newPlayer.ActorNumber}), EEG 프리팹 생성 시작");
        SpawnEEGPrefabForPlayer(newPlayer);
    }

    private void SpawnEEGPrefabForPlayer(Photon.Realtime.Player newPlayer)
    {
        if (eegDisplayPrefab == null)
        {
            Debug.LogError("eegDisplayPrefab이 할당되지 않았습니다!");
            return;
        }

        // 서버가 EEG 프리팹 생성 (PhotonNetwork.Instantiate는 네트워크 상에 동기화됨)
        GameObject eegObj = PhotonNetwork.Instantiate(eegDisplayPrefab.name, Vector3.zero, Quaternion.identity);
        Debug.Log($"서버 EEG 표시용 프리팹 생성됨: 플레이어 ActorNumber={newPlayer.ActorNumber}");

        // 생성된 프리팹의 PhotonView 컴포넌트 가져오기
        PhotonView pv = eegObj.GetComponent<PhotonView>();
        if (pv != null)
        {
            // 소유권을 해당 플레이어에게 이전
            pv.TransferOwnership(newPlayer.ActorNumber);
            Debug.Log($"프리팹 PhotonView 소유권을 플레이어 {newPlayer.ActorNumber}에게 이전했습니다.");
        }
        else
        {
            Debug.LogWarning("생성된 EEG 프리팹에 PhotonView 컴포넌트가 없습니다.");
        }

        playerEEGObjects[newPlayer.ActorNumber] = eegObj;
    }

    [PunRPC]
    public void ReceiveEEGDataFromClient(int attention, int meditation, int blink,
        int delta, int theta, int lowAlpha, int highAlpha,
        int lowBeta, int highBeta, int lowGamma, int highGamma,
        PhotonMessageInfo info)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        int senderActor = info.Sender.ActorNumber;

        if (playerEEGObjects.TryGetValue(senderActor, out GameObject eegObj))
        {
            EEGDataReceiver receiver = eegObj.GetComponent<EEGDataReceiver>();
            if (receiver != null)
            {
                receiver.attention = attention;
                receiver.meditation = meditation;
                receiver.blink = blink;
                receiver.delta = delta;
                receiver.theta = theta;
                receiver.lowAlpha = lowAlpha;
                receiver.highAlpha = highAlpha;
                receiver.lowBeta = lowBeta;
                receiver.highBeta = highBeta;
                receiver.lowGamma = lowGamma;
                receiver.highGamma = highGamma;
            }
            else
            {
                Debug.LogWarning($"EEGDataReceiver 컴포넌트가 서버 EEG 프리팹에 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"플레이어 {senderActor}에 대한 EEG 오브젝트가 서버에 없습니다.");
        }
    }
}
