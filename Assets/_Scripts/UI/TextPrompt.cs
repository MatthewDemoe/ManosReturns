using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for timed prompts. e.g. showing a message on the screen for 10 seconds
/// </summary>

public class TextPrompt : MonoBehaviour
{
    // color of text when time is NOT over
    Color _normalColor;

    // color of text when time is over
    [SerializeField]
    Color flashColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    // text flashes red when near end
    [SerializeField]
    float textFlashFrequency = 5.0f;

    const float UPDATE_FREQ = 0.1f;

    // internal tick-down timer
    float _promptTimeLeft = 0.0f;

    //[Tooltip("self-attached text object")]
    Text textObj { get; set; }

    //// Start is called before the first frame update
    void Start()
    {
        textObj = GetComponent<Text>();
        _normalColor = textObj.color;
    }

    public void PlayPrompt(string message, float duration)
    {
        textObj.text = message;
        StartCoroutine(PropmtCrt(duration));
    }

    IEnumerator PropmtCrt(float duration)
    {
        _promptTimeLeft = duration;
        while (_promptTimeLeft > 0.0f)
        {
            _promptTimeLeft -= Time.deltaTime;
            yield return new WaitForSeconds(UPDATE_FREQ);
        }
        textObj.text = "";
    }

    public void FlashTextColor(Color flashingColor, float duration)
    {
        flashColor = flashingColor;
        InvokeRepeating("FlashColorRepeating", duration, 0.033f);
    }

    void FlashColorRepeating()
    {
        float colLerp = (Mathf.Sin(Time.time * textFlashFrequency) + 1.0f) * 0.5f;
        Color newColor = Color.Lerp(_normalColor, flashColor, colLerp);
        textObj.color = newColor;
    }
}
