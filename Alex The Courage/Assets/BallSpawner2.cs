
using UnityEngine;

public class BallSpawner2 : MonoBehaviour
{
    public GameObject ballPrefab; // Assign your ball prefab in the inspector
    public float spawnRate = 3.0f; // Balls will be spawned every 'spawnRate' seconds

    // Start is called before the first frame update
    void Start()
    {
        // Make the cube invisible
        GetComponent<Renderer>().enabled = false;

        // Start the spawning loop
        StartCoroutine(SpawnLoops());
    }

    IEnumerator SpawnLoops()
    {
        while (true) // Keep spawning forever
        {
            SpawnBalls();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnBalls()
    {
        // Spawn a ball at the cube's position
        Instantiate(ballPrefab, transform.position, Quaternion.identity);
    }
}