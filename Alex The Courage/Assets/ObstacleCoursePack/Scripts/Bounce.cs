using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
	public float force = 10f; //Force 10000f
	public float stunTime = 0.5f;
	private Vector3 hitDir;

	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (collision.gameObject.tag == "Player")
			{
				hitDir = contact.normal;
				collision.gameObject.GetComponent<CharacterControls>().HitPlayer(-hitDir * force, stunTime);
				return;
			}
		}
		/*if (collision.relativeVelocity.magnitude > 2)
		{
			if (collision.gameObject.tag == "Player")
			{
				//Debug.Log("Hit");
				collision.gameObject.GetComponent<CharacterControls>().HitPlayer(-hitDir*force, stunTime);
			}
			//audioSource.Play();
		}*/
	}

	/*void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Bullet"))
		{
			Debug.Log("Collision detected with enemy.");
			EnableRagdoll();
		}
		// Add this block to handle Obstacle collision
		else if (hit.gameObject.CompareTag("Obstacle"))
		{
			Debug.Log("Collision detected with obstacle.");
			EnableRagdoll();
		}
	}*/

    public GameManager gameManager;

    void Start()
    {
        // Initialize GameManager (make sure you've set it up in the Inspector or find it dynamically)
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject hit = collider.gameObject;
        if (hit.CompareTag("Player"))
        {
            PlayerRagdoll playerRagdoll = hit.GetComponent<PlayerRagdoll>();
            if (playerRagdoll != null && !playerRagdoll.IsRagdollEnabled)
            {
                Debug.Log("Enabling Ragdoll");
                playerRagdoll.EnableRagdoll();

                // Invoke the respawn after 3 seconds
                Invoke("TriggerRespawn", 2f);
            }
        }
    }

    // Call this method after 3 seconds to respawn the player
    void TriggerRespawn()
    {
        Debug.Log("Triggering Respawn");
        if (gameManager != null)
        {
            gameManager.EndGame();
        }
    }
}
