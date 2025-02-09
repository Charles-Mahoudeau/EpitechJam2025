using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public AudioClip deathSound;
    public float respawnDelay = 3.0f;
    public Image redFilter;
    private AudioSource audioSource;
    private PlayerMovementScript playerMovementScript;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        playerMovementScript = GetComponent<PlayerMovementScript>();
        if (redFilter != null)
        {
            redFilter.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.name == "FloorDie")
        {
            Die();
        }
    }

    private void Die()
    {
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        if (redFilter != null)
        {
            redFilter.enabled = true;
        }
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
        Invoke("Respawn", deathSound != null ? deathSound.length : respawnDelay);
    }

    private void Respawn()
    {
        transform.position = new Vector3(0, 1.7f, 0);

        if (redFilter != null)
        {
            redFilter.enabled = false;
        }
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }
}
