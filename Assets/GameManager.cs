using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<string> playersLost = new List<string>();

    public Text playerListText; 
    public Text rankingText;

    public int playerCount = 0;
    public int playerDeadCount = 0; 

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

    private void Update()
    {
        playerListText.text = "Verbunden: " + playerCount; 
    }

    private void UpdateRanking()
    {
        // Sortiere die Liste der Spieler, die verloren haben, umgekehrt
        playersLost.Reverse();

        // Baue den Text für das Ranking
        string rankingInfo = "Ranking:\n";
        for (int i = 0; i < playersLost.Count; i++)
        {
            rankingInfo += $"{i + 1}. {playersLost[i]}\n";
        }

        // Aktualisiere die UI
        rankingText.text = rankingInfo;
    }

    public void PlayerDied(string playerName)
    {
        if (!playersLost.Contains(playerName))
        {
            playersLost.Add(playerName);
            UpdateRanking();
        }
    }
}
