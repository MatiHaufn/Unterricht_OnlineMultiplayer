using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public int _currentHealth;

    [SerializeField] int _maxHealth = 3;
    [SerializeField] TextMesh _nickName; 

    PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        slider.maxValue = _maxHealth;
        _currentHealth = _maxHealth; 
        slider.value = _currentHealth;
    }

    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        slider.transform.position = slider.transform.position + new Vector3(0, 35, 0);

        if(_currentHealth <= 0 && _photonView.IsMine)
        {
            _photonView.RPC("SyncPlayerDeath", RpcTarget.All);
        }
    }

    public void ChangeHealth(int value)
    {
        _currentHealth += value;
        _photonView.RPC("SyncPlayerHealth", RpcTarget.All, _currentHealth);
    }


    [PunRPC]
    private void SyncPlayerHealth(int currentHealth)
    {
        slider.value = currentHealth; 
    }

    [PunRPC]
    private void SyncPlayerDeath()
    {
        GameManager.Instance.PlayerDied(_nickName.text);
        PhotonView.Destroy(gameObject); 
    }
}
