using UnityEngine;

public class GoRight : MonoBehaviour
{
    Rigidbody2D body;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        body.linearVelocityX = 30;
    }
}
