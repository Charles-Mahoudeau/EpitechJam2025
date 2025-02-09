using UnityEngine;

public class GroundDestruction : MonoBehaviour
{
    [SerializeField] private new GameObject camera;
    [SerializeField] private GameObject ground;
    private Vector3 _lookingOrientation;
    void Start()
    {
        if (!camera)
            camera = GameObject.Find("Camera");
        if (!camera)
            Debug.LogWarning("No camera found");
        if (!ground)
            ground = GameObject.FindWithTag("groundToDelete");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!ground)
            return;
        if (other.CompareTag("Player"))
        {
            HandleGroundDeletion();
        }
    }

    private void HandleGroundDeletion()
    {
        _lookingOrientation = camera.transform.localRotation.eulerAngles;
        
        if (_lookingOrientation.x is <= 300f and >= 270f)
        {
            Destroy(ground);
        }
    }
}
