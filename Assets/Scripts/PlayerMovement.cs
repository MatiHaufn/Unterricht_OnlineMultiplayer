using Photon.Pun;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerMovement : MonoBehaviour
{
    public TextMesh playerNameText; 
    public float _speed;

    [SerializeField] GameObject _ballTracker; 

    Rigidbody2D _myRigidbody; 
    PhotonView photonView;
    string playerName; 

    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical"); 

        Vector2 move = new Vector2(horizontal, vertical);

        _myRigidbody.velocity = move * _speed; 

        if(Input.GetMouseButtonDown(0) && _ballTracker.GetComponent<BallTracker>()._ballTouched)
        {
            _ballTracker.GetComponent<BallTracker>()._ball.GetComponent<PushBall>().AddForceToBall(transform.position);
        }

    }

    public void SetPlayerName(string name)
    {
        playerName = name; 
        playerNameText.text = name;
    }

    
    [PunRPC]
    public void UpdatePlayerName(string name)
    {
        Debug.Log("Name: " + name); 
        playerNameText.text = name;
    }
}
