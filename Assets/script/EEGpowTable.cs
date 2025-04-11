using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EEGpowTable : MonoBehaviour
{
    public GameObject Alpha;
    public GameObject Beta;
    public GameObject Gamma;
    public GameObject T1;
    public GameObject T2;
    public GameObject T3;
    public GameObject T4;
    private IEnumerator coroutine;

    public int i = -1;
    float A = 0;
    float B = 0;
    float G = 0;
    float Max = 0;

    void Start()
    {
        StartCoroutine(GetEEGpow());
        coroutine = FillText();

    }
    IEnumerator GetEEGpow()
    {
        A = Alpha.GetComponent<InletOutlet2>().EEGpow;
        A = Mathf.Floor(A * 100f) / 100f;
        B = Beta.GetComponent<InletOutlet2>().EEGpow;
        B = Mathf.Floor(B * 100f) / 100f;
        G = Gamma.GetComponent<InletOutlet2>().EEGpow;
        G = Mathf.Floor(G * 100f) / 100f;

        if (A > B && A > G)
            Max = A;
        if (B > G && B > A)
            Max = B;
        if (G > A && G > B)
            Max = G;

        yield return new WaitForSeconds(1f);

        StartCoroutine(FillText());
    }

    IEnumerator FillText()
    {
        if (i != -1 && i <= 10)
        {
            T1.transform.GetChild(i).gameObject.GetComponent<TMP_Text>().text = A.ToString();
            T1.transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color = new Color32(175, 0, 0, 255);
            T2.transform.GetChild(i).gameObject.GetComponent<TMP_Text>().text = B.ToString();
            T2.transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color = new Color32(0, 175, 0, 255);
            T3.transform.GetChild(i).gameObject.GetComponent<TMP_Text>().text = G.ToString();
            T3.transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color = new Color32(0, 0, 175, 255);
            T4.transform.GetChild(i).gameObject.GetComponent<TMP_Text>().text = Max.ToString();
            T4.transform.GetChild(i).gameObject.GetComponent<TMP_Text>().color = new Color32(0, 0, 0, 255);
        }

        i++;

        yield return new WaitForSeconds(1f);

        StartCoroutine(GetEEGpow());
    }
}
