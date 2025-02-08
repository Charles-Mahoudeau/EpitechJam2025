using Unity.VisualScripting;
using UnityEngine;

public class ObjectInteractScript : MonoBehaviour
{
    [SerializeField] private float colliderRadius = 1.0f;
    [SerializeField] private Vector3 holdPosition;
    [SerializeField] private Quaternion holdRotation;
    
    private SphereCollider _collider;
    private GameObject _interactCanvasPrefab;
    private GameObject _interactCanvas;
    
    private void Start()
    {
        _interactCanvasPrefab = Resources.Load<GameObject>("InteractPopup");
        
        _collider = transform.AddComponent<SphereCollider>();
        _collider.center = Vector3.zero;
        _collider.radius = colliderRadius;
        _collider.isTrigger = true;
        
        _interactCanvas = Instantiate(_interactCanvasPrefab, transform.position + new Vector3(0.0f, 0.5f, 0.0f), 
            Quaternion.identity);
        _interactCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        
        SetCanvasActive(false);
    }

    public void SetCanvasActive(bool active)
    {
        _interactCanvas.transform.position = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
        _interactCanvas.SetActive(active);
    }

    public Vector3 GetHoldPosition()
    {
        return holdPosition;
    }

    public Quaternion GetHoldRotation()
    {
        return holdRotation;
    }
}
