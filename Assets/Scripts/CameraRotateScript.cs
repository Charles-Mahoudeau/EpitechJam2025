using UnityEngine;

public class CameraRotateScript : MonoBehaviour
{
    [SerializeField] private float speed = 360.0f;
    
    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
    }
}
