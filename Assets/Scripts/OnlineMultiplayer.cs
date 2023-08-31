using Photon.Pun;
using Photon.Realtime; 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class OnlineMultiplayer : MonoBehaviourPunCallbacks
{
    public static OnlineMultiplayer Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        Debug.Log("Joined Game");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

   
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby called");
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4}, TypedLobby.Default); 
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom called"); 
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector2(5, 0), Quaternion.identity);
        string playerName = LoginManager.PlayerNickName;  
        player.GetComponent<PlayerMovement>().SetPlayerName(playerName);
    }
}
