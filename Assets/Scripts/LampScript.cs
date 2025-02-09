using System.Linq;
using UnityEngine;

public class LampScript : MonoBehaviour, IEquippable
{
    [SerializeField] private float precision = 0.1f;
    [SerializeField] private uint raysCount = 100;
    [SerializeField] private float raysDistance = 50f;
    [SerializeField] private float distanceShadowHeightFactor = 1.0f;
    [SerializeField] private Material shadowMaterial;
    
    private bool _shadowActive;
    private Vector3[] _shadowVertices;
    
    private GameObject _shadow;
    private MeshRenderer _shadowRenderer;
    private BoxCollider _shadowCollider;

    private bool isEquipped = false;
    
    private static Vector3 GetVectors3Center(params Vector3[] vectors)
    {
        if (vectors == null || vectors.Length == 0)
        {
            return Vector3.zero;
        }
        
        var sum = vectors.Aggregate(Vector3.zero, (current, vec) => current + vec);
        
        return sum / vectors.Length;
    }

    private static Vector3 Vector3LockY(Vector3 vec, float y)
    {
        vec.y = y;
        
        return vec;
    }

    private void Start()
    {
        if (!shadowMaterial)
        {
            Debug.LogWarning("No shadow material assigned, using standard shader material.");
            shadowMaterial = new Material(Shader.Find("Standard"));
        }
        
        _shadow = GameObject.CreatePrimitive(PrimitiveType.Quad);
        _shadow.name = "LampShadow";
        _shadow.AddComponent<BoxCollider>();
        _shadow.layer = LayerMask.NameToLayer("Ground");

        _shadowRenderer = _shadow.GetComponent<MeshRenderer>();
        _shadowCollider = _shadow.GetComponent<BoxCollider>();
        
        _shadowRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _shadowRenderer.receiveShadows = false;
        _shadowRenderer.material = shadowMaterial;
        
        var center = _shadowCollider.center;
        center.z += 0.1f;
        _shadowCollider.center = center;
    }

    private void Update()
    {
        UpdateShadow();
        
        _shadow.SetActive(_shadowActive);
        if (_shadowActive)
        {
            UpdateShadowGameObject();
        }
    }

    private float GetShadowWidth(Collider colliderFilter)
    {
        var hitsCount = 0;
        
        for (float i = - (int) (raysCount / 2); i < (int) (raysCount / 2); i++)
        {
            var origin = transform.position + transform.right * (i * precision);
            var hasHit = Physics.Raycast(origin, transform.forward, out var hit, raysDistance,
                LayerMask.GetMask("ShadowWall"));

            if (!hasHit || hit.collider != colliderFilter)
            {
                continue;
            }
            
            hitsCount++;
        }

        var width = (hitsCount - 1) * precision;

        return Mathf.Max(0, width);
    }

    private void UpdateShadow()
    {
        var hasTargetHit = Physics.Raycast(transform.position, transform.forward, out var targetHit,
            raysDistance, LayerMask.GetMask("ShadowWall"));
        var hasGroundHit = Physics.Raycast(transform.position, - transform.up, out var groundHit, 
            raysDistance, LayerMask.GetMask("Ground"));

        _shadowActive = hasTargetHit && hasGroundHit;
        
        if (!_shadowActive)
        {
            return;
        }
        
        var groundY = groundHit.point.y;
        var shadowStart = Vector3LockY(
            GetVectors3Center(targetHit.collider.bounds.min, targetHit.collider.bounds.max), groundY);
        var shadowWidth = GetShadowWidth(targetHit.collider);
        var shadowHeight = targetHit.collider.bounds.max.y * (1.0f / targetHit.distance) * 
            distanceShadowHeightFactor;
        var shadowEnd = shadowStart + Vector3LockY(transform.forward * shadowHeight, 0);

        _shadowVertices = new []
        {
            Vector3LockY(shadowStart - transform.right * (shadowWidth * 0.5f), groundY),
            Vector3LockY(shadowStart + transform.right * (shadowWidth * 0.5f), groundY),
            Vector3LockY(shadowEnd - transform.right * (shadowWidth * 0.5f), groundY),
            Vector3LockY(shadowEnd + transform.right * (shadowWidth * 0.5f), groundY)
        };
    }

    private void UpdateShadowGameObject()
    {
        _shadow.transform.position = GetVectors3Center(_shadowVertices);
        var rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(90.0f, rotation.eulerAngles.y, 0.0f);
        _shadow.transform.rotation = rotation;
        var scale = _shadow.transform.localScale;
        scale.x = Vector3.Distance(_shadowVertices[0], _shadowVertices[1]);
        scale.y = Vector3.Distance(_shadowVertices[0], _shadowVertices[2]);
        _shadow.transform.localScale = scale;
        _shadowCollider.size = new Vector3(1.0f, 1.0f, 0.2f);
    }

    private void OnDrawGizmos()
    {
        UpdateShadow();

        if (_shadowActive)
        {
            DrawShadowGizmos();
        }
    }

    private void DrawShadowGizmos()
    {
        var hasTargetHit = Physics.Raycast(transform.position, transform.forward, out var targetHit,
            raysDistance, LayerMask.GetMask("ShadowWall"));
        
        Gizmos.color = Color.green;

        if (hasTargetHit)
        {
            Gizmos.DrawLine(transform.position, targetHit.point);
        }
        
        foreach (var vert in _shadowVertices)
        {
            Gizmos.DrawSphere(vert, 0.1f);
        }
    }

    public void Unequip()
    {
        isEquipped = false;
    
    }

    public void Equip()
    {
        isEquipped = true;
    
    }
}