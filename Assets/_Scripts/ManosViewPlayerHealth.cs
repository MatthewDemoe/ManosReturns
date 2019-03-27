using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManosViewPlayerHealth : MonoBehaviour {

    PlayerHealth health;

    [SerializeField]
    Transform manos;

    [SerializeField]
    Image hpBar;

    [SerializeField]
    Image hpBack;

    [SerializeField]
    float updateTime;

    float updateTimer;

    float oldValue;

	// Use this for initialization
	void Start () {
        health = GetComponentInParent<PlayerHealth>();

         //if (manos == null)
         //    manos = GameObject.Find("Manos 2.3").transform.GetChild(0).GetChild(2);
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(manos);

        if (updateTimer < updateTime)
        {
            hpBack.fillAmount = Mathf.Lerp(oldValue, health.GetHealthPercentage(), updateTimer / updateTime);
            updateTimer += Time.deltaTime;
        }
	}

    public void UpdateValue(){
        hpBar.fillAmount = health.GetHealthPercentage();
        oldValue = hpBack.fillAmount;
        updateTimer = 0;
    }
}
