using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField]
    float maxStamina = 100.0f;

    [SerializeField]
    float stamina = 100.0f;

    [SerializeField]
    float stamRegenDelay = 1.0f;

    [SerializeField]
    float stamRegenRate = 33.3f;

    [SerializeField]
    StaminaBarUI ui;

    bool _isRegen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetStaminaPercent()
    {
        return stamina / maxStamina;
    }

    public bool UseStamina(float cost)
    {
        if (stamina > cost)
        {
            ui.SetRegenerating(false);

            stamina -= cost;
            ui.UseCharge();

            StopCoroutine("StaminaLogic");

            StartCoroutine("StaminaLogic");

            return true;
        }

        return false;
    }

    IEnumerator StaminaLogic()
    {
        yield return new WaitForSeconds(stamRegenDelay);

        ui.SetRegenerating(true);

        while (stamina < maxStamina)
        {
            stamina += stamRegenRate * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        ui.SetRegenerating(false);
    }
}
