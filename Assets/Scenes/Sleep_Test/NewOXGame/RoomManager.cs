using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject eegDisplayPrefab; // EEG 데이터를 보여줄 서버측 프리팹

    // 플레이어 ActorNumber -> EEG 프리팹 PhotonView.ViewID 매핑
    private Dictionary<int, int> playerToEEGViewID = new Dictionary<int, int>();

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("포톤 네트워크 연결 시도");
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

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"유저 입장 확인, 프리팹 생성");
            SpawnClientPrefab(newPlayer);
        }
    }

    private void SpawnClientPrefab(Photon.Realtime.Player newPlayer)
    {
        if (eegDisplayPrefab == null)
        {
            Debug.LogError("eegDisplayPrefab이 할당되지 않았습니다!");
            return;
        }

        // PhotonNetwork.Instantiate는 네트워크 상에 오브젝트를 생성하고 PhotonView에 ViewID를 자동 부여함
        GameObject eegObj = PhotonNetwork.Instantiate(eegDisplayPrefab.name, Vector3.zero, Quaternion.identity);
        Debug.Log($"클라이언트 EEG 표시용 프리팹 생성 - 플레이어 ActorNumber: {newPlayer.ActorNumber}");

        PhotonView pv = eegObj.GetComponent<PhotonView>();
        if (pv == null)
        {
            Debug.LogError("eegDisplayPrefab에 PhotonView가 없습니다!");
            PhotonNetwork.Destroy(eegObj);
            return;
        }

        playerToEEGViewID[newPlayer.ActorNumber] = pv.ViewID;
    }

    // 클라이언트가 보낸 ViewID를 통해 해당 EEGDataReceiver에게 데이터 전달하기 위한 헬퍼 메서드 예시
    public void ForwardEEGData(int senderActorNumber, int viewID,
        int attention, int meditation, int blink,
        int delta, int theta, int lowAlpha, int highAlpha,
        int lowBeta, int highBeta, int lowGamma, int highGamma)
    {
        if (playerToEEGViewID.TryGetValue(senderActorNumber, out int mappedViewID))
        {
            if (mappedViewID == viewID)
            {
                PhotonView pv = PhotonView.Find(viewID);
                if (pv != null)
                {
                    pv.RPC("ReceiveEEGData", RpcTarget.All,
                        attention, meditation, blink,
                        delta, theta, lowAlpha, highAlpha,
                        lowBeta, highBeta, lowGamma, highGamma);
                }
            }
            else
            {
                Debug.LogWarning($"ViewID 불일치 - 플레이어: {senderActorNumber}, 받은 ViewID: {viewID}, 매핑된 ViewID: {mappedViewID}");
            }
        }
        else
        {
            Debug.LogWarning($"플레이어 ActorNumber {senderActorNumber} 에 대한 ViewID 매핑이 없습니다.");
        }
    }
}
