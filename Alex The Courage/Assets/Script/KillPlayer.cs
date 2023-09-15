using UnityEngine;

public class KillPlayer : MonoBehaviour
{
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
                FindObjectOfType<AudioManager>().Play("Explode");
                TriggerRespawn();
            }
        }
    }

    void TriggerRespawn()
    {
        Debug.Log("Triggering Respawn");
        if (gameManager != null)
        {
            gameManager.EndGame();
        }
    }
}
