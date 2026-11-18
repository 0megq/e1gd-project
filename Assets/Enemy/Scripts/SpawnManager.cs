using System;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] spawnPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        print(spawnPoints[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
