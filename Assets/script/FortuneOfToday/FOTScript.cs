using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class FOTScript : MonoBehaviour
{
    public GameObject Alpha;
    public GameObject Beta;
    public GameObject Theta;
    public GameObject Alpha_result;
    public GameObject Beta_result;
    public GameObject Theta_result;
    float A = 0;
    float B = 0;
    float T = 0;
    int A_point = 0;
    int B_point = 0;
    int T_point = 0;

    void Start()
    {
        StartCoroutine(GetEEGpow());
        StartCoroutine(GetResult());
    }
    IEnumerator GetEEGpow()
    {
        A = Alpha.GetComponent<InletOutlet2>().EEGpow;
        A = Mathf.Floor(A * 100f) / 100f;
        B = Beta.GetComponent<InletOutlet2>().EEGpow;
        B = Mathf.Floor(B * 100f) / 100f;
        T = Theta.GetComponent<InletOutlet2>().EEGpow;
        T = Mathf.Floor(T * 100f) / 100f;

        if (A > 1000)
        {
            A_point++;
        }

        if (B > 1000)
        {
            B_point++;
        }

        if (T > 2000)
        {
            T_point++;
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator GetResult()
    {
        yield return new WaitForSeconds(10f);

        if (A_point > B_point && A_point > T_point)
        {
            PopUpAlpha();
        }

        if (B_point > A_point && B_point > T_point)
        {
            PopUpBeta();
        }

        if (T_point > B_point && T_point > A_point)
        {
            PopUpTheta();
        }

        else
        {
            PopUpTheta();
        }
    }

    void PopUpAlpha()
    {
        Alpha_result.SetActive(true);
        Alpha_result.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    void PopUpBeta()
    {
        Beta_result.SetActive(true);
        Beta_result.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    void PopUpTheta()
    {
        Theta_result.SetActive(true);
        Theta_result.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
}
