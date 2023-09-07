using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public InputField nickNameInput;
    public Button joinButton;

    public static LoginManager Instance;
    public static string PlayerNickName;  // Statische Variable, um den Nicknamen zu speichern

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

    private void Start()
    {
        joinButton.interactable = false;
    }

    public void UpdateNickname()
    {
        joinButton.interactable = !string.IsNullOrEmpty(nickNameInput.text);
    }
    public void OnClickStartGame()
    {
        PlayerNickName = nickNameInput.text;  // Den Wert in der statischen Variable speichern
        SceneManager.LoadScene("Game");
    }
}
