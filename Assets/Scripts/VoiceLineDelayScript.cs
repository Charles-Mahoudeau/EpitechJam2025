using UnityEngine;

public class VoiceLineDelayScript : MonoBehaviour
{
    [SerializeField] private float delay;
    
    private AudioSource _audio;
    
    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        
        Invoke(nameof(PlayAudio), delay);
    }

    private void PlayAudio()
    {
        _audio.Play();
    }
}
