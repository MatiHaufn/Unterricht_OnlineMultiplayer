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

    public void AddForceToBall(Vector3 playerPosition)
    {
        Vector2 hitDirection = (transform.position - playerPosition).normalized;
        rb.velocity = hitDirection; 
        rb.AddForce(hitDirection * forceMultiplyer, ForceMode2D.Impulse);
    }

}
