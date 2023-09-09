using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] Transform enemySpawnPoint;
    [SerializeField] float enemySpawnValue;

    // Update is called once per frame
    void Update()
    {
        if(enemy.transform.position.y < -enemySpawnValue) {
            EnemyRespawnPoint();
        }
    }

    void EnemyRespawnPoint() {
        transform.position = enemySpawnPoint.position;
    }

}

