using Photon.Pun;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 2f;
    Transform player; // Der Spieler, um den herum die Mausbewegung eingeschränkt wird.
    PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        player = this.transform; 
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            RestrictMouseWithinCircle();
        }
    }

    void RestrictMouseWithinCircle()
    {
        // Radius des Kreises, in dem die Mausbewegung eingeschränkt wird
        float radius = 3f;

        // Die Mausposition relativ zur Bildschirmmitte
        Vector3 mousePos = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Berechnen Sie den Winkel der Mausposition
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        // Begrenzen Sie die Mausposition innerhalb des Kreises
        if (mousePos.magnitude > radius)
        {
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            // Setzen Sie die Mausposition neu, um innerhalb des Kreises zu bleiben
            mousePos = new Vector3(x, y, 0);
        }

        // Setzen Sie die Mausposition auf die begrenzte Position zurück
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
