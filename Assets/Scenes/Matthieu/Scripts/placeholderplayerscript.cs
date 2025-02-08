using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float maxVerticalAngle = 89f;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if(cameraTransform.parent != transform)
            cameraTransform.SetParent(transform);
    }

    void Update()
    {
        HandleMouseLook();
        HandleJumpInput();
    }

    void FixedUpdate()
    {
        HandleMovement();
        GroundCheck();
    }

    void HandleMovement()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 move = (transform.forward * input.z + transform.right * input.x) * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            mouseX /= 5;
            mouseY /= 5;
        }
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleJumpInput()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void GroundCheck()
    {
        Vector3 start = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(start, Vector3.down, groundCheckDistance + 0.1f, groundMask);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 start = transform.position + Vector3.up * 0.1f;
        Gizmos.DrawLine(start, start + Vector3.down * (groundCheckDistance + 0.1f));
    }
}