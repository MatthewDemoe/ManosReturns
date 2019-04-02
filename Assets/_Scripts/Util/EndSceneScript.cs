using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneScript : MonoBehaviour
{

    [SerializeField]
    float endAfter = 10.0f;

    [SerializeField]
    float displayTextFor = 3.0f;

    [SerializeField]
    Text txt;

    [SerializeField]
    float textLerpTime = 0.5f;

    [SerializeField]
    Image manosQuad;

    float timer = 0.0f;

    void Start()
    {
        timer = textLerpTime;
        StartCoroutine("DoStuff");
    }

    IEnumerator DoStuff()
    {
        yield return new WaitForSeconds(endAfter);

        txt.enabled = true;

        Color txColor = txt.color;
        txColor.a = 0.0f;

        Color mqColor = manosQuad.color;
        mqColor.a = 0.0f;

        float val;

        while (timer > 0.0f)
        {
            val = UtilMath.Lmap(timer, textLerpTime, 0.0f, 0.0f, 1.0f);

            txColor.a = val;
            txt.color = txColor;

            timer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(displayTextFor);

        timer = textLerpTime;

        while (timer > 0.0f)
        {
            val = UtilMath.Lmap(timer, textLerpTime, 0.0f, 1.0f, 0.0f);

            txColor.a = val;
            txt.color = txColor;

            val = UtilMath.Lmap(timer, textLerpTime, 0.0f, 0.0f, 1.0f);

            mqColor.a = val;
            manosQuad.color = mqColor;

            timer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("Title");
    }
}
