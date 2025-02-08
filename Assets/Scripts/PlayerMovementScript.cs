using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float friction = 1.0f;
    [SerializeField] private float airFriction = 0.2f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private LayerMask groundLayerMask;
    
    private bool _readyToJump;
    
    [Header("Camera")]
    [SerializeField] private float sensitivity = 1.0f;

    private float _rotationY;
    private float _rotationX;

    private Rigidbody _rb;

    private GameObject _camera;
    
    private bool _isGrounded;

    [Header("Ground deletion")]
    [SerializeField] private string groundTag;

    private Component _groundRb;

    private GameObject _ground;

    private Vector3 _lookingOrientation;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        _rb.freezeRotation = true;
        
        _readyToJump = true;
        
        _camera = transform.Find("Camera").gameObject;
        if (groundTag == null)
            groundTag = "groundToDelete";
        if (!_camera)
        {
            Debug.LogWarning("Camera not found");
        }
        _ground = GameObject.FindWithTag(groundTag);
        if (!_ground)
            Debug.LogWarning("No ground with tag " + groundTag);
        _groundRb = _ground.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight);
        
        UpdateCamera();
        MovePlayer();
        if (_ground)
            HandleGroundDeletion();
    }

    private void MovePlayer()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");

        _rb.linearDamping = _isGrounded ? friction : airFriction;
        
        if(Input.GetButton("Jump") && _readyToJump && _isGrounded)
        {
            _readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        
        var moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        _rb.AddForce(moveDirection * (moveSpeed * Time.deltaTime), ForceMode.Force);
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    
    private void ResetJump()
    {
        _readyToJump = true;
    }

    private void UpdateCamera()
    {
        var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        _rotationY += mouseX;
        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(0.0f, _rotationY, 0.0f);
        _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundLayerMask);
        
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * playerHeight);
    }

    private void HandleGroundDeletion()
    {
        _lookingOrientation = _camera.transform.localRotation.eulerAngles;
        
        if (_lookingOrientation.x is <= 300f and >= 270f)
        {
            Destroy(_ground);
        }
    }
}
