using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject housePrefab;


    private void Start()
    {
        Spawn();
    }

    void Spawn() {

        Instantiate(housePrefab);

    }
}