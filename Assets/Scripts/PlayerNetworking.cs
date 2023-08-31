using Photon.Pun;
using UnityEngine;

public class PlayerNetworking : MonoBehaviour
{
    public MonoBehaviour[] scriptsToIgnore;
    PhotonView photonView; 

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if(!photonView.IsMine)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0.23f, 0.19f);

            foreach (var script in scriptsToIgnore) 
            {
                script.enabled = false; 
            }
        }
    }
}
