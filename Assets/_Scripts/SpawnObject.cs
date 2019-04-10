using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField]
    float radius = 50;

    [SerializeField]
    GameObject obj;

    [SerializeField]
    float spawnTime = 10.0f;

    float _timer = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimerUpdate();
    }
    
    void TimerUpdate()
    {
        if (_timer <= 0.0f)
        {
            Spawn();
            _timer = spawnTime;
        }
        _timer -= Time.deltaTime;
    }
    void Spawn()
    {
        Vector2 _twoDimensional = Random.insideUnitCircle*radius;

        Instantiate(obj,transform.position+ new Vector3(_twoDimensional.x, 0.0f,_twoDimensional.y),Random.rotation);

    }
}
