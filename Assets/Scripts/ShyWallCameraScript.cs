using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ShyWallCameraScript : MonoBehaviour
{
    [SerializeField] private float maxDistance = 50.0f;
    [SerializeField] private float fov = 90.0f;
    [SerializeField] private float precision = 1.0f;
    [SerializeField] private float height = 10.0f;
    [SerializeField] private float heightPrecision = 1.0f;
    [SerializeField] private float heightAngle = 1.0f;

    private Dictionary<GameObject, ShyWallScript> _wallsMap;

    private GameObject[] GetVisibleWalls()
    {
        var walls = new List<GameObject>();
        
        for (var delta = - height; delta <= height; delta += heightPrecision)
        {
            for (var angle = - fov * 0.75f; angle <= fov * 0.75f; angle += precision)
            {
                var rotation = Quaternion.AngleAxis(angle, transform.up);
                rotation *= Quaternion.AngleAxis(delta * heightAngle, transform.right);
                
                var hasHit = Physics.Raycast(transform.position, 
                    rotation * transform.forward, out var hit, maxDistance,
                    LayerMask.GetMask("ShyWall"));

                if (!hasHit)
                {
                    continue;
                }
            
                var wall = hit.transform.gameObject;
            
                if (walls.Contains(hit.transform.gameObject))
                {
                    continue;
                }
            
                walls.Add(wall);
            }
        }

        return walls.ToArray();
    }
    
    private void Start()
    {
        var walls = GameObject.FindGameObjectsWithTag("ShyWall");
        
        _wallsMap = new Dictionary<GameObject, ShyWallScript>();

        foreach (var wall in walls)
        {
            var script = wall.GetComponent<ShyWallScript>();

            if (!script)
            {
                throw new WarningException("Unable to find ShyWallScript component");
            }
            
            _wallsMap.Add(wall, script);
        }
    }

    private void Update()
    {
        var visibleWalls = GetVisibleWalls();
        
        foreach (var (_, script) in _wallsMap)
        {
            if (!script)
            {
                continue;
            }
            
            script.SetVisible(false);
        }
        foreach (var wall in visibleWalls)
        {
            ShyWallScript script;
            
            if (_wallsMap.TryGetValue(wall, out var value))
            {
                script = value;
            }
            else
            {
                script = wall.GetComponent<ShyWallScript>();
                _wallsMap.Add(wall, script);
            }
            
            if (!script)
            {
                throw new WarningException("Unable to find ShyWallScript component");
            }
            
            script.SetVisible(true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (var delta = - height; delta <= height; delta += heightPrecision)
        {
            for (var angle = - fov * 0.75f; angle <= fov * 0.75f; angle += precision)
            {
                GizmoRay(transform.position, angle, delta * heightAngle);
            }
        }
    }

    private void GizmoRay(Vector3 origin, float hAngle, float vAngle)
    {
        var rotation = Quaternion.AngleAxis(hAngle, transform.up);
        rotation *= Quaternion.AngleAxis(vAngle, transform.right);
        
        var hasHit = Physics.Raycast(origin, rotation * 
            transform.forward, out var hit, maxDistance, LayerMask.GetMask("ShyWall"));
        
        Gizmos.color = hasHit ? Color.green : Color.red;
        Gizmos.DrawLine(origin, hit.point);
    }
}
