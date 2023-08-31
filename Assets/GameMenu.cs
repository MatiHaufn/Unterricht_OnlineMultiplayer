using UnityEngine;

public class GameMenu : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit(); 
        }
    }
}
