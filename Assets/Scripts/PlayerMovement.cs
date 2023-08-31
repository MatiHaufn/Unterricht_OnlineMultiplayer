using Photon.Pun;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerMovement : MonoBehaviour
{
    public TextMesh playerNameText; 
    public float _speed;
 
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
