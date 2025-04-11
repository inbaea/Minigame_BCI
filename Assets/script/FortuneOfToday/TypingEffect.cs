using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TypingEffect : MonoBehaviour
{
    public TMP_Text tmp;
    public string m_tmp;
    void Start()
    {
        StartCoroutine(FillText());
        m_tmp = tmp.text;
        tmp.text = "";
    }

    IEnumerator FillText()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < m_tmp.Length; i++)
        {
            tmp.text = m_tmp.Substring(0, i);

            yield return new WaitForSeconds(0.1f);
        }   
    }
}
