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
        // Setze die Netzwerkraten
        PhotonNetwork.SendRate = 20; // Wie oft die Daten gesendet werden (in Millisekunden)
        PhotonNetwork.SerializationRate = 10; // Wie oft die Daten empfangen werden (in Millisekunden)
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
        player.GetComponent<PlayerMovement>().SetPlayerName(LoginManager.PlayerNickName);
        player.GetComponent<PhotonView>().RPC("UpdatePlayerName", RpcTarget.AllBuffered, LoginManager.PlayerNickName);

        GameManager.Instance.playerCount = PhotonNetwork.PlayerList.Length;
        photonView.RPC("SyncPlayerCount", RpcTarget.All, GameManager.Instance.playerCount);

        int currentHealth = player.GetComponent<HealthBar>()._currentHealth;
        Debug.Log(player.name + ": " + currentHealth);
        player.GetComponent<PhotonView>().RPC("SyncPlayerHealth", RpcTarget.AllBuffered, currentHealth);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} with ID {newPlayer.ActorNumber} entered the room.");

        // Inkrementiere den Zähler für die Anzahl der Spieler
        GameManager.Instance.playerCount++;

        // Synchronisiere den Spielerzähler über das Netzwerk
        photonView.RPC("SyncPlayerCount", RpcTarget.All, GameManager.Instance.playerCount);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.NickName} with ID {otherPlayer.ActorNumber} left the room.");

        // Dekrementiere den Zähler für die Anzahl der Spieler
        GameManager.Instance.playerCount--;

        // Synchronisiere den Spielerzähler über das Netzwerk
        photonView.RPC("SyncPlayerCount", RpcTarget.All, GameManager.Instance.playerCount);
    }

    [PunRPC]
    private void SyncPlayerCount(int count)
    {
        GameManager.Instance.playerCount = count;
    }
}
