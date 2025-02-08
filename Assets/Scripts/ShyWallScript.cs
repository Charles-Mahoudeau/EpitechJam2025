using UnityEngine;

public class ShyWallScript : MonoBehaviour
{
    private MeshRenderer _renderer;
    private BoxCollider _collider;
    
    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<BoxCollider>();
        
        SetVisible(true);
    }

    public void SetVisible(bool visible)
    {
        _renderer.enabled = visible;
        _collider.isTrigger = !visible;
    }
}
