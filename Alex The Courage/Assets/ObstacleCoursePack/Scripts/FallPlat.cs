using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float waitTimeBeforeFall = 2.0f; // Time in seconds to wait before the platform starts falling
    private Rigidbody rb;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;  // Initially, the platform should not be affected by gravity
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide with player");
        // If the platform is already falling, ignore further collisions
        if (isFalling)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // Start the coroutine to make the platform fall
            StartCoroutine(FallAfterSeconds(waitTimeBeforeFall));
        }
    }

    IEnumerator FallAfterSeconds(float seconds)
    {
        // Wait for the specified time
        yield return new WaitForSeconds(seconds);

        // Now make the platform fall
        rb.isKinematic = false;
        isFalling = true; // Set the flag to ignore further collisions
    }
}