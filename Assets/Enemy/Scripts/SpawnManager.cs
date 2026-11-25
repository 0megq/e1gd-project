using System;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{


    [SerializeField] BaseEnemy[] enemyTypes;

    [Header("Wave formula")]
    [SerializeField] int baseEnemies = 3;
    [SerializeField] float growthPerWave = 2f;
    [SerializeField] float timeBetweenWaves = 5f;
    [SerializeField] float spawnInterval = 0.4f;

    int aliveEnemies;
    private GameObject[] spawnPoints;

    public int WaveNum { get; private set; } = 0;
    public event System.Action<int> WaveStarted;

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
