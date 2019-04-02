using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BarShakeController : MonoBehaviour
{
    [SerializeField]
    GameObject background;

    [SerializeField]
    GameObject container;

    RectTransform _trans;

    Vector3 _scal;

    Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _trans = GetComponent<RectTransform>();
    }

    public void SetChargeAmount(float val)
    {
        if (val > 0.0f)
        {
            background.SetActive(true);
            container.SetActive(true);
        }

        else
        {
            background.SetActive(false);
            container.SetActive(false);
        }
        _scal = _trans.localScale;
        _scal.y = val;
        _trans.localScale = _scal;

        _anim.SetFloat("ChargeAmount", val);
    }

    public void SetColor(Color c)
    {
        GetComponent<Image>().color = c;
    }


}
