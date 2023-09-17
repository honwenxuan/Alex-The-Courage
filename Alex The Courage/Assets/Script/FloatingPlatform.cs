using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float speed = 0.3f;  // Speed at which the platform moves
    public float distance = 50f; // Distance between the two points
    public Vector3 direction = new Vector3(0, 0, -1); // Direction of movement

    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 lastPosition; // Store last position to calculate the delta
    private Transform playerTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        endPoint = startPoint + direction * distance;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }

    // Move the platform between startPoint and endPoint
    void MovePlatform()
    {
        float time = Mathf.PingPong(Time.time * speed, 1);
        Vector3 newPosition = Vector3.Lerp(startPoint, endPoint, time);
        Vector3 deltaPosition = newPosition - lastPosition; // Get the delta between the last and new position

        // Update last position for the next frame
        lastPosition = transform.position = newPosition;

        // Move the player along with the platform
        if (playerTransform != null)
        {
            playerTransform.position += deltaPosition;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerTransform = null;
        }
    }
}