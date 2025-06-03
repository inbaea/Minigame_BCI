using UnityEngine;
using TMPro;

public class EEGProfileUI : MonoBehaviour
{
    public EEGDataReceiver targetReceiver;

    public TMP_Text attentionText;
    public TMP_Text meditationText;
    public TMP_Text blinkText;

    void Update()
    {
        if (targetReceiver != null)
        {
            attentionText.text = $"Attention: {targetReceiver.attention}";
            meditationText.text = $"Meditation: {targetReceiver.meditation}";
            blinkText.text = $"Blink: {targetReceiver.blink}";
        }
    }
}
