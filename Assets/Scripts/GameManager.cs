using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    PhotonView photonView;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] Button readyButton;
    int readyCount = 0; 

    [System.Serializable]
    public class PlayerStatus
    {
        public string playerName;
        public bool isReady;
    }

    private List<PlayerStatus> playerStatusList = new List<PlayerStatus>();

    private List<string> playersLost = new List<string>();

    public Text playerListText;
    public Text readyText;
    public Text rankingText;
    
    public int playerCount = 0;
    public int playerDyingCount = 0;

    bool gameStarted = false;
    bool isReady = false;

    string localPlayerName; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnClickStartGame()
    {
        readyCount++; 
        isReady = true;
        readyButton.interactable = false; 
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (PhotonNetwork.IsConnected)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.IsLocal)
                {
                    localPlayerName = player.NickName;
                    break;
                }
            }
        }

        playerStatusList.Add(new PlayerStatus { playerName = localPlayerName, isReady = false });

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName != localPlayerName)
            {
                playerStatusList.Add(new PlayerStatus { playerName = player.NickName, isReady = false });
            }
        }

        readyText.text = "Ready: " + isReady;
    }

    private void Update()
    {
        playerListText.text = "Verbunden: " + playerCount;

        if (playersLost.Count == playerCount - 1 && gameStarted)
        {
            Debug.Log("WIN");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("ready");
            readyText.text = "Ready: " + isReady;
            isReady = !isReady;
            photonView.RPC("SetPlayerReady", RpcTarget.All, localPlayerName, isReady);
        }

        if (AllPlayersReady() && !gameStarted)
        {
            StartGame();
        }

    }

    [PunRPC]
    public void SetPlayerReady(string playerName, bool isReady)
    {
        // Suche nach dem Spielerstatus in der Liste
        PlayerStatus playerStatus = playerStatusList.Find(p => p.playerName == playerName);

        // Überprüfe, ob der Spielerstatus gefunden wurde
        if (playerStatus != null)
        {
            playerStatus.isReady = isReady;
        }
        else
        {
            Debug.LogError($"Player {playerName} not found in playerStatusList.");
        }
    }


    private bool AllPlayersReady()
    {
        int readyPlayerCount = 0;

        foreach (var playerStatus in playerStatusList)
        {
            if (playerStatus.isReady)
            {
                readyPlayerCount++;
            }
            else 
            {
                return false; // Wenn ein Spieler nicht bereit ist, ist die Bedingung nicht erfüllt
            }
        }
        return readyPlayerCount == playerStatusList.Count;
    }

    private void StartGame()
    {
        // Hier startest du das Spiel, z.B., indem du den Ball spawnst.
        Debug.Log("Spiel gestartet!");
        if (!gameStarted)
            ballPrefab.SetActive(true); 
        gameStarted = true;
    }

    private void UpdateRanking()
    {
        playersLost.Reverse();

        string rankingNames = "Ranking:\n"; 

        for (int i = 0; i < playersLost.Count; i++)
        {
            rankingNames += $"{i + 1}. {playersLost[i]}\n";
        }

        rankingText.text = rankingNames;
    }

    public void PlayerDied(string playerName)
    {
        if (!playersLost.Contains(playerName))
        {
            playersLost.Add(playerName);
            playerDyingCount++; 
            Debug.Log(playersLost);
            UpdateRanking();
        }
    }
}
