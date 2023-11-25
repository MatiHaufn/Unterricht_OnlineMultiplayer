using Photon.Pun;
using UnityEngine;

public class PushBall : MonoBehaviour
{
    PhotonView photonView;
    Rigidbody2D rb;

    float forceMultiplyer = 5.0f;
    float maxForce = 20;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();

    }

    private void Update()
    {
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity -= rb.velocity.normalized * Time.deltaTime;
        }
    }


    public void AddForceToBall(Vector3 playerPosition)
    {
        Vector2 hitDirection = (transform.position - playerPosition).normalized;

        if (rb.velocity.magnitude < maxForce)
            rb.velocity = rb.velocity.magnitude * hitDirection;
        else
            rb.velocity = maxForce * hitDirection; 
        
        rb.AddForce(hitDirection * forceMultiplyer, ForceMode2D.Impulse);

        photonView.RPC("UpdateBallMovement", RpcTarget.All, rb.velocity, transform.position);
    }

    [PunRPC]
    private void UpdateBallMovement(Vector2 velocity, Vector3 position)
    {
        transform.position = position;
        rb.velocity = velocity;
    }
}
