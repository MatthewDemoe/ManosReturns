using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public class ButtonMash : MonoBehaviour
{
    
    
    Color _startC = Color.white;

    [SerializeField]
    Color _endC;

    Image img;

    [SerializeField]
    float maxTime = 0.3f;

    float _time = 0.0f;

    bool _pressed = false;
   
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        img.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonMashPrompt()
    {
        img.enabled = true;
        _time += Time.deltaTime;

        if(_time >= maxTime)
        {
            if (_pressed)
            {
                img.color = _endC;

            }
            else
            {
                img.color = _startC;
            }

            _pressed = !_pressed;
                _time = 0.0f;
        }

    }

    public void Reset()
    {
        _time = 0.0f;
        img.color = _startC;
        img.enabled = false;
    }

}
