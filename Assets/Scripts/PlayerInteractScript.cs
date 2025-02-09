using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractScript : MonoBehaviour
{
    [SerializeField] private float range = 10.0f;

    private Dictionary<GameObject, ObjectInteractScript> _cache;
    private GameObject _currentGameObject;
    private ObjectInteractScript _currentInteractScript;

    private GameObject _camera;
    private GameObject PlayerCamera
    {
        get
        {
            if (!_camera)
            {
                _camera = transform.Find("Camera").gameObject;
            }
            return _camera;
        }
    }

    private InputAction _interactAction;

    private GameObject _currentHoldObject;
    
    private void Start()
    {
        _cache = new Dictionary<GameObject, ObjectInteractScript>();

        _interactAction = InputSystem.actions.FindAction("Interact");

        _currentGameObject = null;
        _currentInteractScript = null;
        _currentHoldObject = null;
    }
    
    private void Update()
    {
        UpdateInput();
        
        var hasHit = Physics.Raycast(PlayerCamera.transform.position, 
            PlayerCamera.transform.forward, out var hit, range, 
            LayerMask.GetMask("Interactable"));

        if (!hasHit)
        {
            _currentInteractScript?.SetCanvasActive(false);
            
            _currentGameObject = null;
            _currentInteractScript = null;
            
            return;
        }

        if (_currentGameObject == hit.transform.gameObject)
        {
            return;
        }
        
        _currentInteractScript?.SetCanvasActive(false);
        
        _currentGameObject = hit.transform.gameObject;
        _currentInteractScript = GetInteractScript(_currentGameObject);
        
        _currentInteractScript.SetCanvasActive(true);
    }

    private void UpdateInput()
    {
        if (_interactAction.WasPressedThisFrame() && (_currentGameObject || _currentHoldObject))
        {
            var inventory = GetComponent<PlayerInventory>();

            if (_currentHoldObject)
            {
                _currentHoldObject.transform.parent = transform.parent;
                

            if (inventory != null)
            {
                inventory.UnequipItem(inventory.currentItemIndex);
                inventory.RemoveEquippedItem(_currentHoldObject);
            }

            _currentHoldObject = null;
            }
            else
            {
                if (!_currentGameObject)
                {
                    throw new Exception("Invalid state");
                }
                
                _currentGameObject.transform.parent = transform;
                _currentGameObject.transform.localPosition = _currentInteractScript.GetHoldPosition();
                _currentGameObject.transform.localRotation = _currentInteractScript.GetHoldRotation();
                
                _currentHoldObject = _currentGameObject;
                
                if (inventory != null)
                {
                    inventory.AddEquippedItem(_currentGameObject);
                    int newIndex = inventory.items.Count - 1;
                    inventory.UnequipItem(inventory.currentItemIndex);
                    inventory.currentItemIndex = newIndex;
                    inventory.EquipItem(newIndex);
                }
            }
        }
    }

    private ObjectInteractScript GetInteractScript(GameObject target)
    {
        if (_cache.TryGetValue(target, out var script))
        {
            return script;
        }
        
        script = target.GetComponent<ObjectInteractScript>();
        _cache.Add(target, script);

        if (!script)
        {
            throw new WarningException("Unable to find ObjectInteractScript on target " + target);
        }
        
        return script;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(PlayerCamera.transform.position,
            PlayerCamera.transform.position + PlayerCamera.transform.forward * range);
    }
}
