using UnityEngine;

public class Wave2 : MonoBehaviour
{
    Rigidbody2D rb;
    float speed;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed = Random.Range(40f, 60f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 getVel = new Vector3(1f, 0, 0) * -1 * speed;
        rb.linearVelocity = getVel;
    }
}
