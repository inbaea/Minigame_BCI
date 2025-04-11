using UnityEngine;
using TMPro;
public class NewTableMaker : MonoBehaviour
{
    public GameObject EEGpow;    // 뇌파 값 받는 스크립트
    public GameObject T1;
    public GameObject T2;

    public float MAX_EEG = 0;
    public float AVR_EEG = 0;
    
    private int times = 0;
    private bool isPlay = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float AVR;

        if (isPlay)
        {
            float EEG = EEGpow.GetComponent<InletOutlet2>().EEGpow;
        
            if (EEG > MAX_EEG)
            {
                MAX_EEG = EEG;
            }

            AVR_EEG += EEG;
            times++;
        }

        if (times == 2000)
        {
            AVR = CalAVR();
            AVR_EEG = AVR;
            FillText();
        }
    }

    public float CalAVR()
    {
        float AVR;
        AVR = AVR_EEG / times;
        return AVR;
    }

    public void FillText()
    {
        if (T1.name == "Alpha")
        {
            T1.GetComponent<TMP_Text>().text = MAX_EEG.ToString();
            T1.GetComponent<TMP_Text>().color = new Color32(175, 0, 0, 255);
            T2.GetComponent<TMP_Text>().text = AVR_EEG.ToString();
            T2.GetComponent<TMP_Text>().color = new Color32(175, 0, 0, 255);
        }

        if (T1.name == "Beta")
        {
            T1.GetComponent<TMP_Text>().text = MAX_EEG.ToString();
            T1.GetComponent<TMP_Text>().color = new Color32(0, 175, 0, 255);
            T2.GetComponent<TMP_Text>().text = AVR_EEG.ToString();
            T2.GetComponent<TMP_Text>().color = new Color32(0, 175, 0, 255);
        }

        if (T1.name == "SMR")
        {
            T1.GetComponent<TMP_Text>().text = MAX_EEG.ToString();
            T1.GetComponent<TMP_Text>().color = new Color32(0, 0, 175, 255);
            T2.GetComponent<TMP_Text>().text = AVR_EEG.ToString();
            T2.GetComponent<TMP_Text>().color = new Color32(0, 0, 175, 255);
        }
    }
}
