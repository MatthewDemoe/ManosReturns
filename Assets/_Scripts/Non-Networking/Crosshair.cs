using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {

    [SerializeField]
    RectTransform rectTrans;

    [SerializeField]
    Texture2D img;
    [SerializeField]
    float size;
    [SerializeField]
    float maxSize;
    [SerializeField]
    float offset;
    [SerializeField]
    float sizeIncrease;
    [SerializeField]
   Camera cam;
    [SerializeField]
    GameObject cameraPrefab;
    [SerializeField]
    Vector3 screenPos;

    [SerializeField]
    Vector3 forw;

    [SerializeField]
    bool isHit;
    [SerializeField]
    float setTime;

    [SerializeField]
    float rot;
    [SerializeField]
    float rotSpeed;
    float sizeDecay;
    [SerializeField]
    Vector3 campos;

    //Transform cam;
    void Start()
    {

    }
    void Update()
    {
        int layerMask = LayerMask.GetMask("Player","Projectile");
        layerMask = ~layerMask;
        //cam = Camera.main;
        forw = transform.position + transform.forward + new Vector3(0.0f, 1.0f, 0.0f);
       // campos = cam.transform.position;
        RaycastHit hitter;
        
        isHit = Physics.Raycast(forw, cam.transform.forward, out hitter, Mathf.Infinity, layerMask);
        if (isHit)
        {
            screenPos = cam.WorldToScreenPoint(hitter.point);
        }
        else if (!isHit)
        {
            screenPos = cam.WorldToScreenPoint(forw+ cam.transform.forward*100.0f);
        }
        float newSize = size;
        //if (cont.isFiring())
        //{
        //    newSize = maxSize;
        //    sizeDecay = setTime;
        //}
        //if (sizeDecay > 0&& !cont.isFiring())
        //{
        //    sizeDecay -= Time.deltaTime;
        //    newSize = Mathf.Lerp(size, maxSize, sizeDecay / setTime);
        //   
        //}

        rectTrans.localScale = new Vector3(newSize, newSize, 0);
        rectTrans.position = new Vector3(screenPos.x, screenPos.y,0);
        rot += Time.deltaTime * rotSpeed;
        rectTrans.localRotation = Quaternion.Euler(0,0,rot);
       // GUI.DrawTexture(new Rect(screenPos.x-(newSize*0.5f), screenPos.y - (newSize * 0.5f), newSize, newSize), img);

    }

    /*public void LookHeight(float val)
    {
        lookHeight+=value

    }
    // Use this for initialization
   
	
	// Update is called once per frame
	void Update () {
		
	}*/
}
