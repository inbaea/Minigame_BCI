using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colored : MonoBehaviour
{
    float time = 0f;
    float F_time = 1f;

    Image image;

    void Start()
    {
        image = this.GetComponent<Image>();
        StartCoroutine(Fadeflow());
    }

    IEnumerator Fadeflow()
    {
        time = 0f;
        Color alpha = image.color;
        yield return new WaitForSeconds(0.5f);

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            image.color = alpha;
            yield return null;
        }

        yield return new WaitForSeconds(15.5f);

        time = 0f;
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            image.color = alpha;
            yield return null;
        }

        yield return null;
    }
}
