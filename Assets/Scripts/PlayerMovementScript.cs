using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private float _acceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float friction = 1.0f;
    [SerializeField] private float airFriction = 0.2f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float gravityForce = 200f;
    [SerializeField] private LayerMask groundLayerMask;
    
    private bool _readyToJump;
    
    [Header("Camera")]
    [SerializeField] private float sensitivity = 1.0f;

    private float _rotationY;
    private float _rotationX;

    private Rigidbody _rb;

    private GameObject _camera;
    
    private bool _isGrounded;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        _rb.freezeRotation = true;
        
        _readyToJump = true;
        
        _camera = transform.Find("Camera").gameObject;
        if (!_camera)
        {
            Debug.LogWarning("Camera not found");
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundLayerMask);
        
        UpdateCamera();
        MovePlayer();
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
        if (_rb.linearVelocity.y <= 0 && !_isGrounded)
        {
            var increaseYForce = new Vector3(0f, gravityForce, 0f);
            _rb.AddForce(increaseYForce * (Time.deltaTime), ForceMode.Force);
        }
        var moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        _rb.AddForce(moveDirection * (Time.deltaTime * moveSpeed), ForceMode.Force);
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
}