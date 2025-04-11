using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Change1 : MonoBehaviour
{
    public GameObject child;
    RectTransform rt;
    float time = 0f;
    float F_time = 3f;

    public float x;
    public float y;
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        StartCoroutine(Recall());
    }

    IEnumerator Recall()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(RandomChange());
        StartCoroutine(Recall());
    }

    IEnumerator RandomChange()
    {
        time = 0f;
        x = Random.Range(250, 650);
        y = x - (float)37.5;

        Vector2 curr = rt.sizeDelta;

        if (rt.sizeDelta.x < x)
        {
            while (rt.sizeDelta.x < x)
            {
                time += Time.deltaTime / F_time;
                curr.x = Mathf.Lerp(curr.x, x, time);
                curr.y = Mathf.Lerp(curr.y, y, time);
                rt.sizeDelta = curr;
                child.GetComponent<RectTransform>().sizeDelta = curr / 3;
                yield return null;
            }
        }
        else
        {
            while (rt.sizeDelta.x > x)
            {
                time += Time.deltaTime / F_time;
                curr.x = Mathf.Lerp(curr.x, x, time);
                curr.y = Mathf.Lerp(curr.y, y, time);
                rt.sizeDelta = curr;
                child.GetComponent<RectTransform>().sizeDelta = curr / 3;
                yield return null;
            }
        }

        yield return null;
    }
}
