using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManosEnergyFist : MonoBehaviour
{
    [SerializeField]
    Color slowColor;

    [SerializeField]
    Color fastColor;

    [SerializeField]
    float currentVel;

    [SerializeField]
    float maxVelThreshold;

    [SerializeField]
    Material mat;

    int propID = 0;

    ManosHand hand;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        propID = Shader.PropertyToID("_EmissionColor");
        hand = GetComponentInParent<ManosHand>();
    }

    // Update is called once per frame
    void Update()
    {
        currentVel = hand.GetVelocity().magnitude;
        mat.SetColor(propID, Color.Lerp(slowColor, fastColor, currentVel / maxVelThreshold));
    }
}
