using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private bool isFullscreen = true;
    private void Start()
    {
        Cursor.visible = true;
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Cursor.lockState = CursorLockMode.Confined; 
            Cursor.visible = false; 
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit(); 
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleFullscreen();
        }
    }

    void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen; 
    }
}
