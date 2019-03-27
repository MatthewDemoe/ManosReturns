using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballFactory : MonoBehaviour
{
    [SerializeField]
    List<GameObject> spawnPoints;

    [SerializeField]
    float spawnTime = 10.0f;

    [SerializeField]
    GameObject pickup;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnFootballs", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnFootballs()
    {
        int spawnPoint = Random.Range(0, spawnPoints.Count - 1);

        Instantiate(pickup, spawnPoints[spawnPoint].transform);
    }
}
