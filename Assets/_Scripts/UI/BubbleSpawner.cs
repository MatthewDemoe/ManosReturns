using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BubbleSpawner : MonoBehaviour
{
    public Camera manosCam;
    public Camera chadCam;
    public GameObject bubblePrefab;
 //   public GameObject dManagerHolder;
    [SerializeField]
    DialogManager man;
    // Start is called before the first frame update
    void Start()
    {
       // man = dManagerHolder.GetComponent<DialogManager>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBubble(string text)
    {
        
        GameObject spawnedBubbles = Instantiate(bubblePrefab, chadCam.transform.position+new Vector3(1.22f, 0.9000001f, 2.59f),new Quaternion(),chadCam.transform);
        BubbleScript bubble = spawnedBubbles.GetComponentInChildren<BubbleScript>();
        bubble.SetText(man, text);
        bubble.BeginUpdate();


    }
}
