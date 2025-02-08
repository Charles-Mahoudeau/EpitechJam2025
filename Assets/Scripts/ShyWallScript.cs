using System;
using UnityEngine;

public class ShyWallScript : MonoBehaviour
{
    private MeshRenderer _renderer;
    private BoxCollider _collider;
    private Camera _camera;
    
    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<BoxCollider>();
        
        _camera = Camera.main;

        if (!_camera)
        {
            throw new Exception("Camera not found");
        }
        
        SetVisible(true);
    }

    private void Update()
    {
        var directionToPlayer = transform.position - _camera.transform.position;
        var playerAngle = Vector3.SignedAngle(transform.forward, directionToPlayer, Vector3.up);
        var angle = Vector3.SignedAngle(transform.forward, _camera.transform.forward, Vector3.up);

        Debug.Log(angle);
        
        if (playerAngle < 0)
        {
            SetVisible(angle is <= 45 or >= 135);
        }
        else
        {
            SetVisible(angle is <= -135 or >= -45);
        }
    }

    private void SetVisible(bool visible)
    {
        _renderer.enabled = visible;
        _collider.isTrigger = !visible;
    }
}
