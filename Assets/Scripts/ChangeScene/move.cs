using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public Rigidbody rb;     // Reference to Rigidbody (optional)

    private Vector3 movement;

    void Start()
    {
        // Get the Rigidbody component if not assigned
        if (rb == null) 
            rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input from ZQSD keys
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) moveZ = 1f; // Forward
        if (Input.GetKey(KeyCode.S)) moveZ = -1f; // Backward
        if (Input.GetKey(KeyCode.A)) moveX = -1f; // Left
        if (Input.GetKey(KeyCode.D)) moveX = 1f; // Right

        movement = new Vector3(moveX, 0f, moveZ).normalized;
    }

    void FixedUpdate()
    {
        // If using Rigidbody, apply force-based movement
        if (rb != null)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
        else
        {
            // Otherwise, move directly using transform
            transform.position += movement * speed * Time.deltaTime;
        }
    }
}