using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDab : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField]
    List<Renderer> bodyParts;

    [Header("Damaged Feedback Properties")]
    [SerializeField]
    float timeOn = 0.04f;

    [SerializeField]
    float timeOff = 0.04f;

    [SerializeField]
    int numFlashes = 3;

    int _dabFlashCounter = 0;

    // Start is called before the first frame update


    public void DabFlash()
    {
        StartCoroutine("Flash");
    }

    IEnumerator Flash()
    {
       

        while (_dabFlashCounter < numFlashes)
        {
            _dabFlashCounter++;

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].enabled = false;
            }

            yield return new WaitForSeconds(timeOn);

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].enabled = true;
            }

            yield return new WaitForSeconds(timeOff);

        }



        _dabFlashCounter = 0;
    }
}
