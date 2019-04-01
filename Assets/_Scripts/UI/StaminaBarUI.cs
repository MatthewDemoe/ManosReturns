using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles one Stamina bar
/// </summary>
/// 
public class StaminaBarUI : MonoBehaviour
{
    [SerializeField]
    List<RectTransform> staminaCharges;

    [SerializeField]
    RectTransform chargingBar;

    [SerializeField]
    GameObject chad;

    PlayerStamina _chadStam;

    bool _isRegenerating = false;

    float chargePercentage;

    void Start()
    {
        _chadStam = chad.GetComponent<PlayerStamina>();

        chargePercentage = 1.0f / staminaCharges.Count;
    }

    void Update()
    {
        if (_isRegenerating)
        {
            ScaleBar();

            StartCoroutine("CheckCharges");
        }
    
    }

    void ScaleBar()
    {
        Vector3 scal = chargingBar.localScale;
        scal.x = _chadStam.GetStaminaPercent();
        chargingBar.localScale = scal;
    }

    IEnumerator CheckCharges()
    {
        float stam = _chadStam.GetStaminaPercent();

        for (int i = staminaCharges.Count; i > 0; i--)
        {
            if (stam >= chargePercentage * i)
            {
                if (!staminaCharges[staminaCharges.Count - i].gameObject.activeInHierarchy)
                {
                    staminaCharges[staminaCharges.Count - i].gameObject.SetActive(true);

                    yield return new WaitForEndOfFrame();

                    staminaCharges[staminaCharges.Count - i].gameObject.GetComponent<Animator>().SetTrigger("Recharged");

                    break;
                }
            }
        }

    }

    public void SetRegenerating(bool reg)
    {
        _isRegenerating = reg;
    }

    public void UseCharge()
    {
        for (int i = 0; i < staminaCharges.Count; i++)
        {
            if (staminaCharges[i].gameObject.activeInHierarchy == true)
            {
                staminaCharges[i].gameObject.SetActive(false);



                break;
            }
        }

        ScaleBar();
    }
}
