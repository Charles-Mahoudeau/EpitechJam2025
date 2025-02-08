using UnityEngine;

public class BillboardUIScript : MonoBehaviour
{
    private Canvas _canvas;
    private Camera _camera;
    
    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        _camera = _canvas.worldCamera;
    }
    
    private void Update()
    {
        _canvas.transform.LookAt(2.0f * _canvas.transform.position - _camera.transform.position);
    }
}
