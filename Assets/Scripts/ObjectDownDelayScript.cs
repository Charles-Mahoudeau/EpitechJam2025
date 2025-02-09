using UnityEngine;

public class WallDownDelayScript : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float speed = 1.0f;
    
    private bool _isDown;

    private void Start()
    {
        Invoke(nameof(Down), delay);
    }
    
    private void Update()
    {
        if (_isDown)
        {
            transform.Translate(Vector2.down * (Time.deltaTime * speed));
        }
    }

    private void Down()
    {
        _isDown = true;
    }
}
