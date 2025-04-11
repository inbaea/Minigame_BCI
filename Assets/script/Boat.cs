using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    RectTransform rt;
    int i = 1;
    public int k = 0;
    public bool inside = false;

    public GameObject repeat;
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        StartCoroutine(BoatRepeat());
    }

    private void Update()
    {
        if (k > 200)
        {
            repeat.GetComponent<StopNRepeat>().StopGame();
        }
    }

    IEnumerator BoatRepeat()
    {
        if (i == 1)
        {
            for (int j = 1; j < 60; j++)
            {
                if (rt.rotation.z < 0.3)
                {
                    yield return new WaitForSeconds(0.1f);
                    Vector3 getVel = new Vector3(0, 0, 1) * i;
                    rt.Rotate(getVel);
                }

                else break;
            }
        }

        if (i == -1)
        {
            for (int j = 1; j < 60; j++)
            {
                if (rt.rotation.z > -0.3)
                {
                    yield return new WaitForSeconds(0.1f);
                    Vector3 getVel = new Vector3(0, 0, 1) * i;
                    rt.Rotate(getVel);
                }

                else break;
            }
        }

        i = i * -1;

        StartCoroutine(BoatRepeat());
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name == "Wave")
        {
            inside = true;
            StartCoroutine(WaveStay());
        }
        if (coll.gameObject.name == "Wave_big")
        {
            inside = true;
            StartCoroutine(WaveStay());
        }
    }

    public void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.name == "Wave")
        {
            inside = true;
        }
        if (coll.gameObject.name == "Wave_big")
        {
            inside = true;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.name == "Wave")
        {
            inside = false;
            StartCoroutine(WaveExit());
        }
        if (coll.gameObject.name == "Wave_big")
        {
            inside = false;
            StartCoroutine(WaveExit());
        }
    }

    IEnumerator WaveStay()
    {   
        yield return new WaitForSeconds(0.1f);
        if (inside)
        {
            k += 1;
        }
        StartCoroutine(WaveStay());
    }

    IEnumerator WaveExit()
    {
        yield return new WaitForSeconds(0.1f);
        if (!inside)
        {
            if (k > -10)
            {
                k -= 1;
            }
        }
        StartCoroutine(WaveExit());
    }
}
