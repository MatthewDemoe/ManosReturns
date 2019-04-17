using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAreaBounds : MonoBehaviour
{
    [SerializeField]
    GameObject Manos;

    [SerializeField]
    Image getBackInTheFightImage;

    [SerializeField]
    Text getBackInTheFightText;

    [SerializeField]
    float playAreaX = 1;

    [SerializeField]
    float playAreaZ = 1;


    // Start is called before the first frame update
    void Start()
    {
        getBackInTheFightImage.gameObject.SetActive(true);
        getBackInTheFightText.CrossFadeAlpha(0, 0.1f, false);
    }

    // Update is called once per frame
    void Update()
    {

        if ((Mathf.Abs(Manos.transform.localPosition.x) >= playAreaX) || (Mathf.Abs(Manos.transform.localPosition.z) >= playAreaZ))
        {
            // make the message visible
            getBackInTheFightImage.CrossFadeAlpha(1.0f, 0.1f, false);
            getBackInTheFightText.CrossFadeAlpha(1.0f, 0.1f, false);
        }
        else
        {
            // make the message invisible
            getBackInTheFightImage.CrossFadeAlpha(0, 1.5f, false);
            getBackInTheFightText.CrossFadeAlpha(0, 0.1f, false);
        }
    }
}
