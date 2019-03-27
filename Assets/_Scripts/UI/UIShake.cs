using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIShake : MonoBehaviour
{
    [SerializeField]
    public float magnitude = 5.0f;

    [SerializeField]
    public float duration = 5.0f;

    [SerializeField]
    float shakeDecayRate = 1.2f;

    [SerializeField]
    float updateFrequency = 0.033f;

    [SerializeField]
    float shakeSmoothing = 0.1f;

    // used for text bubble shake
    private float _decayingShakeMagnitude;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Shakes the rectTransform for a given period and magnitude override
    /// </summary>
    public void Shake(float duration, float magnitude)
    {
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        _decayingShakeMagnitude = magnitude;
        InvokeRepeating("RepeatingShake", duration, updateFrequency);
    }

    /// <summary>
    /// Shakes the rectTransform for the configured period and magnitude
    /// </summary>
    public void Shake()
    {
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        _decayingShakeMagnitude = magnitude;
        InvokeRepeating("RepeatingShake", duration, updateFrequency);
    }

    // private method used by ShakeRect
    private void RepeatingShake()
    {
        RectTransform trans = GetComponent<RectTransform>();

        Vector3 newRandomPosition = new Vector3(Random.Range(-_decayingShakeMagnitude, _decayingShakeMagnitude), Random.Range(-_decayingShakeMagnitude, _decayingShakeMagnitude), trans.localPosition.z);

        trans.localPosition =
            Vector3.Lerp(
                newRandomPosition,
                trans.localPosition,
                shakeSmoothing
            );

        if(Mathf.Abs(shakeDecayRate) > Mathf.Epsilon)
        {
            _decayingShakeMagnitude /= shakeDecayRate;
        }
    }
}
