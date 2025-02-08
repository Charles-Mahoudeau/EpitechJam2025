using UnityEngine;

public class VoicelineDelayScript : MonoBehaviour
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
