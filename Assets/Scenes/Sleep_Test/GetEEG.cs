using UnityEngine;
using TMPro;
public class GetEEG : MonoBehaviour
{
    public TMP_Text Text_A;

    void Update()
    {
        Text_A.text = gameObject.GetComponent<InletOutlet2>().EEGpow.ToString();
    }
}
