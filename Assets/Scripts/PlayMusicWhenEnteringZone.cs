using UnityEngine;

public class PlayMusicWhenEnteringZone : MonoBehaviour
{
  private bool _checkAudio;
  [SerializeField] private AudioSource clip;
  [SerializeField] private bool isPlayedOnce = true;
  [SerializeField] private bool playOnlyAtExit = false;
  private void Start()
  {
      _checkAudio = false;
  }

  private void OnTriggerEnter(Collider other)
  {
      if (other.CompareTag("Player") && !_checkAudio && !playOnlyAtExit)
      {
          _checkAudio = isPlayedOnce;
          clip.Play();
      }
  }

  private void OnTriggerExit(Collider other)
  {
      if (other.CompareTag("Player") && !_checkAudio)
      {
          _checkAudio = isPlayedOnce;
          clip.Play();
      }
  }
}
