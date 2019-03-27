using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUserInterface : MonoBehaviour
{
    [SerializeField]
    PlayerHealth manosHP;
    [SerializeField]
    PlayerHealth pOneHP;

   

    // [SerializeField]
    // PlayerHealth pTwoHP;
    // [SerializeField]
    // PlayerHealth pThreeHP;

    [SerializeField]
    Image mHP;
    [SerializeField]
    Image mDmgImg1;
    [SerializeField]
    Image mDmgImg2;
    [SerializeField]
    Image pOneImg;

    // [SerializeField]
    // Image pTwoImg;
    // [SerializeField]
    // Image pThreeImg;

    [SerializeField]
    ManosViewPlayerHealth manosUIOne;

    [SerializeField]
    float updateTime = 0.0f;

    float mUpdateTimer = 1;

    float mOldValue;

    // Use this for initialization
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (mUpdateTimer < updateTime)
        {
            mDmgImg1.fillAmount = Mathf.Lerp(mOldValue, manosHP.GetHealthPercentage(), mUpdateTimer / updateTime);
            Debug.Log(mDmgImg1.fillAmount);
            mUpdateTimer += Time.deltaTime;
        }
    }

    public void KillYouAreSelf()
    {
        //if (manosHP) mHP.gameObject.SetActive(false);
        //if (pOneHP) pOneImg.gameObject.SetActive(false);
        // if (pTwoHP) pTwoImg.gameObject.SetActive(false);
        // if (pThreeHP) pThreeImg.gameObject.SetActive(false);
    }

    public void DealDamage(Enums.Player p)
    {
        switch (p) {
            case Enums.Player.Manos:

                break;
        }
    }

    public void RegisterPlayer(PlayerHealth _health)
    {
        //if (net == null) net = GameObject.Find("OverlordController").GetComponent<NetStateManager>();
        //switch (net.activePlayers)
        //{
        //    case 0:
        //        manosHP = _health;
        //        break;
        //    case 1:
        //        manosHP = GameObject.Find("Manos 2.0(Clone)").GetComponent<PlayerHealth>();
        //        pOneHP = _health;
        //        pOneImg.transform.parent.gameObject.SetActive(true);
        //        print("Enabled a healthbar!");
        //        break;
        //    case 2:
        //        manosHP = GameObject.Find("Manos 2.0(Clone)").GetComponent<PlayerHealth>();
        //        pOneHP = GameObject.Find("Player 2").GetComponent<PlayerHealth>();
        //        pTwoHP = _health;
        //        pTwoImg.transform.parent.gameObject.SetActive(true);
        //        print("Enabled second healthbar!");
        //        break;
        //    case 3:
        //        manosHP = GameObject.Find("Manos 2.0(Clone)").GetComponent<PlayerHealth>();
        //        pOneHP = GameObject.Find("Player 2").GetComponent<PlayerHealth>();
        //        pTwoHP = GameObject.Find("Player 3").GetComponent<PlayerHealth>();
        //        pThreeHP = _health;
        //        pThreeImg.transform.parent.gameObject.SetActive(true);
        //        print("Enabled third healthbar!");
        //        break;
        //}
    }

    public void UpdatePlayerHealth()
    {
        if (pOneHP != null) 
        {
            pOneImg.fillAmount = pOneHP.GetHealthPercentage();
            manosUIOne.UpdateValue();
        }
        
        if (mHP != null)
        {
            mHP.fillAmount = manosHP.GetHealthPercentage();
            mOldValue = mDmgImg1.fillAmount;
            mUpdateTimer = 0;
        }
        
        if (pOneHP.GetHealthPercentage() <= 0.0f)
        {
            Debug.Log("Chad Death");
        }
        
        if (manosHP.GetHealthPercentage() <= 0.0f)
        {
            Debug.Log("Manos Death");
        }

        // if (pTwoHP != null)pTwoImg.fillAmount = pTwoHP.GetHealthPercentage();
        // if (pThreeHP != null) pThreeImg.fillAmount = pThreeHP.GetHealthPercentage();
    }
}
