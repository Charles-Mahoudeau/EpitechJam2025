using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ShyWallScript : MonoBehaviour
{
    private MeshRenderer _renderer;
    private BoxCollider _collider;
    private Camera _camera;

    [SerializeField] private bool zPlusMode;
    [SerializeField] private bool displayShadow;
    
    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<BoxCollider>();
        
        _camera = Camera.main;

        if (!_camera)
        {
            throw new Exception("Camera not found");
        }

        if (!displayShadow)
        {
            _renderer.shadowCastingMode = ShadowCastingMode.Off;
        }
        
        SetVisible(true);
    }

    private void Update()
    {
        var directionToPlayer = transform.position - _camera.transform.position;
        var playerAngle = Vector3.SignedAngle(transform.forward, directionToPlayer, Vector3.up);
        var angle = Vector3.SignedAngle(transform.forward, _camera.transform.forward, Vector3.up);

        if (zPlusMode)
        {
            if (playerAngle is >= -90.0f and <= 90.0f)
            {
                SetVisible(angle is <= 135.0f and >= -135.0f);
            }
            else
            {
                SetVisible(angle is <= -45.0f or >= 45.0f);
            }
        }
        else
        {
            if (playerAngle < 0)
            {
                SetVisible(angle is <= 45 or >= 135);
            }
            else
            {
                SetVisible(angle is <= -135 or >= -45);
            }
        }
    }

    private void SetVisible(bool visible)
    {
        _renderer.enabled = visible;
        _collider.isTrigger = !visible;
    }
}
