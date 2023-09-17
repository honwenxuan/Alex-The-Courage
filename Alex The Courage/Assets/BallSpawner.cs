using System.Collections;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab; // Assign your ball prefab in the inspector
    public float spawnRate = 2.0f; // Balls will be spawned every 'spawnRate' seconds

    // Start is called before the first frame update
    void Start()
    {
        // Make the cube invisible
        GetComponent<Renderer>().enabled = false;

        // Start the spawning loop
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true) // Keep spawning forever
        {
            SpawnBall();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnBall()
    {
        // Spawn a ball at the cube's position
        Instantiate(ballPrefab, transform.position, Quaternion.identity);
    }
}