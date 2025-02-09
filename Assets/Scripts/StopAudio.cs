using System;
using UnityEngine;

public class StopAudio : MonoBehaviour
{
    [SerializeField] private AudioSource clip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && clip)
            AudioSource.Destroy(clip);
    }
}
