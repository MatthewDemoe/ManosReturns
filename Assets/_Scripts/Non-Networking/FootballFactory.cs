using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballFactory : MonoBehaviour
{
    [SerializeField]
    List<GameObject> spawnPoints;

    [SerializeField]
    float spawnRadius = 2.0f;

    [SerializeField]
    float spawnTime = 1.0f;

    [SerializeField]
    int maxSpawned = 20;
    int currentSpawned = 0;

    [SerializeField]
    GameObject pickup;    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginSpawning()
    {
        InvokeRepeating("SpawnFootballs", spawnTime, spawnTime);
    }

    private void SpawnFootballs()
    {
        if (currentSpawned < maxSpawned)
        {
            int spawnPoint = Random.Range(0, spawnPoints.Count);
            
            Instantiate(pickup, spawnPoints[spawnPoint].transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0.0f, Random.Range(-spawnRadius, spawnRadius)), Quaternion.identity);
            currentSpawned++;
        }
    }

    public void PickedUp()
    {
        currentSpawned--;
    }
}
