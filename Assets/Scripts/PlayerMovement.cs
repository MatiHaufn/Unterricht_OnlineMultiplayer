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

    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            photonView.RPC("UpdatePlayerName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
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


        //Berechnung der Mausposition 
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; 

        Vector3 directionToMouse = transform.position - mousePosition;

        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetPlayerName(string name)
    {
        playerNameText.text = name;
    }

    [PunRPC]
    public void UpdatePlayerName(string name)
    {
        playerNameText.text = name;
    }
}
