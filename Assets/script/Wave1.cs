using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave1 : MonoBehaviour
{
    Rigidbody2D rb;
    float speed;
    int i = 1;

    void OnEnable()
    {
        GameObject[] otherObjects = GameObject.FindGameObjectsWithTag("wave");


        for (int i = 0; i < otherObjects.Length; i++)
        {
            Physics2D.IgnoreCollision(otherObjects[i].GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed = Random.Range(15f, 20f);
        StartCoroutine(WaveRepeat());
    }

    IEnumerator WaveRepeat()
    {
        Vector3 getVel = new Vector3(0, 1f, 0) * speed * i;
        rb.linearVelocity = getVel;
        i = i * -1;
        yield return new WaitForSeconds(3f);

        StartCoroutine(WaveRepeat());
    }
}
