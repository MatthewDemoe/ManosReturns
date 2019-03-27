using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles one HP bar
/// </summary>
/// 
public class HPBarUI : MonoBehaviour
{
    [SerializeField]
    RectTransform barHPForeground;

    [SerializeField]
    RectTransform barDamageDealt;

    // time before damage starts to drain
    [SerializeField]
    float damageTimePause = 0.5f;

    [SerializeField]
    float barCatchupSpeed = 0.5f;

    [SerializeField]
    RectTransform plotArmor;

    [SerializeField]
    float plotArmorPercentage = 0.1f;

    [SerializeField]
    UIShake barShake;

    [SerializeField]
    bool shake;

    // current HP state (percentage)
    public float _hpPercentage = 1.0f;

    // where the white (damaged) portion of the HP bar begins
    private float _damagePercentage = 1.0f;
    private float _damagePercentageTimer = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    GetComponent<Animator>().SetBool("Dire", true);
        //}

        if (_damagePercentageTimer > 0.0f)
        {
            _damagePercentageTimer = Mathf.Max(_damagePercentageTimer - Time.deltaTime, 0.0f);
        } else
        {
            //drain the bar
            _damagePercentage = Mathf.Max(_hpPercentage, _damagePercentage - (barCatchupSpeed * Time.deltaTime));
        }

        // update bars
        Vector3 scal = barHPForeground.localScale;
        scal.x = _hpPercentage;
        barHPForeground.localScale = scal;

        Vector3 scalDam = barDamageDealt.localScale;
        scalDam.x = _damagePercentage;
        barDamageDealt.localScale = scalDam;

    }

    /// <summary>
    /// HP bar reacts to taking damage
    /// </summary>
    /// <param name="damagePercent"></param>
    public void DealDamagePercentage(float hp)
    {
        _hpPercentage -= hp;
        _hpPercentage = Mathf.Max(_hpPercentage, 0.0f);

        _damagePercentageTimer = damageTimePause;
        if (shake)
        {
            barShake.Shake(barShake.duration, 1.0f + hp * barShake.magnitude);
        }
    }

    public void SetHitPointPercentage(float hp)
    {
        _hpPercentage = hp;
    }
}
