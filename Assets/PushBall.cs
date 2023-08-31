using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBall : MonoBehaviour
{
    float forceMultiplyer = 5.0f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Vector2 hitDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(hitDirection * forceMultiplyer, ForceMode2D.Impulse);
        }    
    }
}
