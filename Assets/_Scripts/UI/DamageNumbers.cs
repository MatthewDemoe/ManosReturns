using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamageNumbers : MonoBehaviour
{
    [SerializeField]
    float speechBubbleStayTime = 2.0f;

    [SerializeField]
    GameObject damageNumberObject;

    [SerializeField]
    Text text;

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowDamage(float damage)
    {        
        StartCoroutine(DamageBubble(damage));
    }

    IEnumerator DamageBubble(float damage)
    {
        float timer = speechBubbleStayTime;

        string myCurrentText = ((int)damage).ToString() + "!";
        GameObject tmp = Instantiate(damageNumberObject, transform);

        text = tmp.GetComponentInChildren<Text>();
        text.text = myCurrentText;

        while (timer > 0.0f)
        {
            float alpha = UtilMath.Lmap(timer, speechBubbleStayTime, 0.0f, 1.5f, 0.0f);

            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(tmp);
    }
}
