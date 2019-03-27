using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class OutlinePost : MonoBehaviour {
 
    public Shader SimpleOutline;
 
private Material material;
    public float fade;
    public Color edge;
    [Range(0.0f, 1.0f)]  public float Threshold;
    public Color background;

    void Start()

    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;

      /*  if (!SystemInfo.supportsImageEffects || null == material ||
        null == material.shader || !material.shader.isSupported)
        {
            enabled = false;
            return;
        }

        if (SimpleOutline)
        {
            material = new Material(SimpleOutline);

            material.name = "ImageEffectMaterial";
            material.hideFlags = HideFlags.HideAndDontSave;
        }
       
        else
        {
            Debug.LogWarning(gameObject.name + ": Shader is not assigned. Disabling image effect.", this.gameObject);
            enabled = false;
        }*/

    }
    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SimpleOutline = Shader.Find("Hidden/SimpleOutline");
        material = new Material(SimpleOutline);
        material.SetFloat("_BgFade", fade);
        material.SetColor("_EdgeColor", edge);
        material.SetFloat("_Threshold", Threshold);
        material.SetColor("_BgColor", background);
        Graphics.Blit(source, destination, material);
        

   

    }

    void OnDisable()
    {
        if (material)
        {
            DestroyImmediate(material);
        }
    }
}
