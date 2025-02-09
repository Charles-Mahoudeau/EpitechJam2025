using UnityEngine;
using UnityEngine.UI;

public class BlinkingLightScript : MonoBehaviour
{
    [SerializeField] private float upperValue;
    [SerializeField] private float minValue;
    
    private Light _light;

    private bool _active;
    
    private void Start()
    {
        _light = GetComponent<Light>();
    }
    
    private void Update()
    {
        var randomValue = Random.Range(0.0f, upperValue);

        if (randomValue >= minValue)
        {
            ToggleLight();
        }
    }

    private void ToggleLight()
    {
        _active = !_active;
        _light.enabled = _active;
    }
}
