using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public TextMesh _playerNameText;
    public float _speed;
    public float _maxSpeed;

    [SerializeField] GameObject _ballTracker;

    Rigidbody2D _myRigidbody;
    PhotonView _photonView;
    string _playerName;

    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical) * _speed;

        transform.position += (Vector3)move * _speed * Time.deltaTime;

        // Reduzieren Sie die Geschwindigkeit allmählich.
        if (_myRigidbody.velocity.magnitude > 0)
        {
            _myRigidbody.velocity -= _myRigidbody.velocity.normalized * _maxSpeed * Time.deltaTime;
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
        //Debug.Log("Name: " + name);
        _playerNameText.text = name;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            GetComponent<HealthBar>().ChangeHealth(-1);
        }
    }

    [PunRPC]
    private void SyncPlayerMovement(Vector2 velocity, Vector3 position)
    {
        _myRigidbody.velocity = velocity;
        transform.position = position;
    }
}
