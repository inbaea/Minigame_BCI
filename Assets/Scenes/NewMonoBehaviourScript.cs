using UnityEngine;
using System.Collections;

public class shoot : MonoBehaviour
{
    public float Power = 5f;
    private Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DelayedJump());
        }
    }

    IEnumerator DelayedJump()
    {
        yield return new WaitForSeconds(3f); // 3초 기다림
        rigid.AddForce(Vector2.up * Power, ForceMode2D.Impulse);
    }
}
