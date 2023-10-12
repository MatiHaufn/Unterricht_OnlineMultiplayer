using Photon.Pun;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerMovement : MonoBehaviour
{
    public TextMesh _playerNameText; 
    public float _speed;

    [SerializeField] GameObject _ballTracker; 
    
    Rigidbody2D _myRigidbody; 
    PhotonView _photonView;
    string _playerName; 

    bool _bounceFromBall = false;
    float _forceMultiplyer = 5.0f;  
    float _maxForce = 20.0f;  

    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical"); 
        Vector2 move = new Vector2(horizontal, vertical) * _speed;

        transform.position += new Vector3(move.x, move.y, 0) * Time.deltaTime;
        
        if(_bounceFromBall == true && move == Vector2.zero )
        {
            _bounceFromBall = false;
            _myRigidbody.velocity = Vector2.zero;   
        }

        if (_myRigidbody.velocity.magnitude > 0)
        {
            _myRigidbody.velocity -= _myRigidbody.velocity.normalized * Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && _ballTracker.GetComponent<BallTracker>()._ballTouched)
        {
            _ballTracker.GetComponent<BallTracker>()._ball.GetComponent<PushBall>().AddForceToBall(transform.position);
        }
    }

    public void SetPlayerName(string name)
    {
        _playerName = name; 
        _playerNameText.text = name;
    }

    
    [PunRPC]
    public void UpdatePlayerName(string name)
    {
        Debug.Log("Name: " + name); 
        _playerNameText.text = name;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            AddForceToPlayer(other.gameObject.transform.position);
            _bounceFromBall = true; 
        }
    }

    void AddForceToPlayer(Vector3 ballPosition)
    {
        Vector2 hitDirection = (transform.position - ballPosition).normalized;

        if (_myRigidbody.velocity.magnitude < _maxForce)
            _myRigidbody.velocity = _myRigidbody.velocity.magnitude * hitDirection;
        else
            _myRigidbody.velocity = _maxForce * hitDirection;

        _myRigidbody.AddForce(hitDirection * _forceMultiplyer, ForceMode2D.Impulse);

        _photonView.RPC("SyncPlayerMovement", RpcTarget.All, _myRigidbody.velocity, transform.position);
    }

    [PunRPC]
    private void SyncPlayerMovement(Vector2 velocity, Vector3 position)
    {
        _myRigidbody.velocity = velocity;
        transform.position = position;
    }
}
