using UnityEngine;
using Photon.Pun;

public class EEGDataReceiver : MonoBehaviourPun
{
    public int attention, meditation, blink;
    public int delta, theta, lowAlpha, highAlpha;
    public int lowBeta, highBeta, lowGamma, highGamma;

    [PunRPC]
    public void ReceiveEEGData(
        int attention, int meditation, int blink,
        int delta, int theta, int lowAlpha, int highAlpha,
        int lowBeta, int highBeta, int lowGamma, int highGamma)
    {
        this.attention = attention;
        this.meditation = meditation;
        this.blink = blink;
        this.delta = delta;
        this.theta = theta;
        this.lowAlpha = lowAlpha;
        this.highAlpha = highAlpha;
        this.lowBeta = lowBeta;
        this.highBeta = highBeta;
        this.lowGamma = lowGamma;
        this.highGamma = highGamma;

        Debug.Log($"[EEGDataReceiver] 데이터 수신 - attention: {attention}, meditation: {meditation}");
    }
}
