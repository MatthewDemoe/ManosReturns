using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPlacement : MonoBehaviour
{
    [SerializeField]
    Image header;

    [SerializeField]
    Image footer;

    // Start is called before the first frame update
    void Start()
    {
        header.rectTransform.localPosition.Set(0.0f, Screen.height, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
