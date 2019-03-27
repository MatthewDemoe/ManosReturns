using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEmissionMaterial : MonoBehaviour
{
    [SerializeField]
    Material emissiveMaterial;

    [SerializeField]
    SkinnedMeshRenderer mesh;

    /// <summary>
    /// Time in seconds to hit one extreme on the color spectrum, the lower the faster
    /// </summary>
    [SerializeField]
    [Tooltip("Time in seconds to hit one extreme on the color spectrum, the lower the faster")]
    float flashSpeed;

    Material defaultMat;

    float lerpTimer;

    [SerializeField]
    bool flip;

    int propID = 0;

    // Start is called before the first frame update
    void Start()
    {
        defaultMat = mesh.material;
        //foreach(string s in emissiveMaterial.GetTexturePropertyNames())
        //{
        //    print(s);
        //}
        propID = Shader.PropertyToID("_EmissionColor");
        flip = false;
    }

    private void OnDisable()
    {
        emissiveMaterial.SetColor(propID, Color.black);
    }

    private void OnValidate()
    {
        CancelInvoke("FlipTheLerp");
        InvokeRepeating("FlipTheLerp", 0, flashSpeed);
    }

    void FlipTheLerp()
    {
        flip = !flip;
        lerpTimer = 0;
    }

    public void Activate()
    {
        mesh.material = emissiveMaterial;
    }

    public void Deactivate()
    {
        mesh.material = defaultMat;
    }

    // Update is called once per frame
    void Update()
    {
        float lerpValue = lerpTimer / flashSpeed;
        float colourVal = 0;
        switch (flip) {
            case false:
                colourVal = Mathf.Lerp(1, 0, lerpValue);
                break;
            case true:
                colourVal = Mathf.Lerp(0, 1, lerpValue);
                break;
        }

        lerpTimer += Time.deltaTime;
        emissiveMaterial.SetColor(propID, new Color(colourVal, colourVal, colourVal));
    }

}
