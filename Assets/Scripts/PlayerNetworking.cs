using Photon.Pun;
using UnityEngine;

public class PlayerNetworking : MonoBehaviour
{
    public MonoBehaviour[] _scriptsToIgnore;
    public GameObject[] _objToIgnore; 
    PhotonView _photonView; 

    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        if(!_photonView.IsMine)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0.23f, 0.19f);

            foreach (var script in _scriptsToIgnore) 
            {
                script.enabled = false; 
            }
            foreach(GameObject gameObj in _objToIgnore)
            {
                gameObj.SetActive(false);
            }
        }
    }
}
