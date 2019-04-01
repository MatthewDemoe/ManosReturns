using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManosTutorializer : MonoBehaviour
{
    [SerializeField]
    NoNetManos manos;

    [SerializeField]
    Text fistTextStatus;
    [SerializeField]
    Text gunTextStatus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (manos.IsFistPowered(Enums.Hand.Left) || manos.IsFistPowered(Enums.Hand.Right))
        {
            if (manos.IsFistActioning(Enums.Hand.Left) || manos.IsFistActioning(Enums.Hand.Right))
            {
                fistTextStatus.text = "<color=green>Charged!</color>";
            }
            if (manos.IsGunActioning(Enums.Hand.Left) || manos.IsGunActioning(Enums.Hand.Right))
            {
                gunTextStatus.text = "<color=green>Charged!</color>";
            }
        }
        else
        {
            fistTextStatus.text = "<color=red>Not Charged</color>";
            gunTextStatus.text = "<color=red>Not Charged</color>";
        }
    }
}
