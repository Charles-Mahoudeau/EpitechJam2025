using UnityEngine;

public class PlayMusicWhenEnteringZone : MonoBehaviour
{
  private bool _checkAudio;
  [SerializeField] private AudioSource clip;
  
  private void Start()
  {
      _checkAudio = false;
  }

  private void OnTriggerEnter(Collider other)
  {
      if (other.CompareTag("Player") && !_checkAudio)
      {
          _checkAudio = true;
          clip.Play();
      }
  }
}
