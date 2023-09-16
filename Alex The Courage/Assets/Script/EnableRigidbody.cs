using System.Collections.Generic;
using UnityEngine;

public class EnableRi : MonoBehaviour
{
    public List<GameObject> ballObjects = new List<GameObject>(); // List of ball GameObjects

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player has reached the checkpoint, enable Rigidbody for all ball objects
            foreach (GameObject ballObject in ballObjects)
            {
                Rigidbody ballRigidbody = ballObject.GetComponent<Rigidbody>();
                if (ballRigidbody != null)
                {
                    ballRigidbody.isKinematic = false;
                    ballRigidbody.useGravity = true;
                }
            }
        }
    }
}
