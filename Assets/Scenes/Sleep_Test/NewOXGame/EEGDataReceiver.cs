using UnityEngine;
using Photon.Pun;

public class EEGDataReceiver : MonoBehaviourPun, IPunObservable
{
    public int attention;
    public int meditation;
    public int blink;
    public int delta, theta, lowAlpha, highAlpha;
    public int lowBeta, highBeta, lowGamma, highGamma;

    private EEGReceiver eegReceiver;

    void Start()
    {
#if UNITY_2023_1_OR_NEWER
        eegReceiver = Object.FindFirstObjectByType<EEGReceiver>();
#else
        eegReceiver = FindObjectOfType<EEGReceiver>();
#endif

        if (eegReceiver == null)
        {
            Debug.LogWarning("[EEGDataReceiver] 씬 내 EEGReceiver가 없습니다. 데이터 수신 불가.");
        }
    }

    void Update()
    {
        // 이 프리팹이 자기 자신 소유 (즉, 로컬 클라이언트에서만)
        if (photonView.IsMine && eegReceiver != null)
        {
            // 씬에 있는 EEGReceiver에서 값을 가져와 업데이트
            attention = eegReceiver.attention;
            meditation = eegReceiver.meditation;
            blink = eegReceiver.blink;
            delta = eegReceiver.delta;
            theta = eegReceiver.theta;
            lowAlpha = eegReceiver.lowAlpha;
            highAlpha = eegReceiver.highAlpha;
            lowBeta = eegReceiver.lowBeta;
            highBeta = eegReceiver.highBeta;
            lowGamma = eegReceiver.lowGamma;
            highGamma = eegReceiver.highGamma;
        }
    }

    // PhotonView가 자동으로 호출하는 네트워크 동기화 함수
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 내 데이터를 서버 및 다른 클라이언트로 보냄
            stream.SendNext(attention);
            stream.SendNext(meditation);
            stream.SendNext(blink);
            stream.SendNext(delta);
            stream.SendNext(theta);
            stream.SendNext(lowAlpha);
            stream.SendNext(highAlpha);
            stream.SendNext(lowBeta);
            stream.SendNext(highBeta);
            stream.SendNext(lowGamma);
            stream.SendNext(highGamma);
        }
        else
        {
            // 다른 클라이언트(또는 서버)로부터 데이터 수신
            attention = (int)stream.ReceiveNext();
            meditation = (int)stream.ReceiveNext();
            blink = (int)stream.ReceiveNext();
            delta = (int)stream.ReceiveNext();
            theta = (int)stream.ReceiveNext();
            lowAlpha = (int)stream.ReceiveNext();
            highAlpha = (int)stream.ReceiveNext();
            lowBeta = (int)stream.ReceiveNext();
            highBeta = (int)stream.ReceiveNext();
            lowGamma = (int)stream.ReceiveNext();
            highGamma = (int)stream.ReceiveNext();
        }
    }
}
