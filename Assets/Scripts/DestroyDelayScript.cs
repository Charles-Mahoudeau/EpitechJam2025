using UnityEngine;

public class DestroyDelayScript : MonoBehaviour
{
    [SerializeField] private float delay;
    
    private void Start()
    {
        Invoke(nameof(DestroyObject), delay);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
