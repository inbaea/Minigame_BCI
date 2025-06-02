using UnityEngine;
using UnityEngine.UI;

public class EEGDisplay : MonoBehaviour
{
    public int actorNumber;

    public Text statusText;

    public void UpdateData(EEGData data)
    {
        statusText.text = $"[Client {actorNumber}]\n" +
                          $"Attention: {data.attention}\n" +
                          $"Meditation: {data.meditation}\n" +
                          $"Blink: {data.blink}";
        // 필요 시 더 많은 값도 표시 가능
    }
}
