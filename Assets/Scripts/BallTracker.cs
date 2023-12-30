using UnityEngine;

public class BallTracker : MonoBehaviour
{
    public GameObject _ball; 
    public bool _ballTouched;

    [SerializeField] Color _detectedColor; 
    [SerializeField] Color _undetectedColor;

    SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _ball = GameObject.Find("BallPrefab"); 
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            _spriteRenderer.color = _detectedColor;
            _ball = collision.gameObject; 
            _ballTouched = true; 
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            _spriteRenderer.color = _undetectedColor;
            _ballTouched = false; 
        }
    }
}
