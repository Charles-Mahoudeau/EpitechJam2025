using System.Collections;
using UnityEngine;

public class BlinkingLightTrigger : MonoBehaviour
{
    [Header("Light Settings")]
    public Light targetLight;
    public float initialDelay = 0.2f;
    public int numberOfBlinks = 3;
    public float blinkInterval = 0.4f;

    private bool hasBeenTriggered = false;

    private void Start()
    {
        if (targetLight != null)
            targetLight.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && other.CompareTag("Player"))
        {
            hasBeenTriggered = true;
            StartCoroutine(ActivateAndBlinkLight());
        }
    }

    private IEnumerator ActivateAndBlinkLight()
    {
        targetLight.enabled = true;
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < numberOfBlinks; i++)
        {
            Debug.Log("HI");    
            targetLight.enabled = !targetLight.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        targetLight.enabled = true;
    }
}