using System;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] spawnPoints;

    [SerializeField] BaseEnemy[] enemyTypes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawn(int wave)
    {
        int groupSize = wave;
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            int type = UnityEngine.Random.Range(0,enemyTypes.Length);
            for(int j = 0; j < groupSize; j++)
            {   
                BaseEnemy enemy = Instantiate(enemyTypes[type], spawnPoints[i].transform.position, Quaternion.identity);
            }
        }
    }
}
