using UnityEngine;

public class Knockback : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;

    //knockback
    private Vector3 knockbackDirection;
    private float knockbackDuration = 0.5f;  // 0.5 seconds, change as needed
    private float knockbackSpeed = 15f;  // speed of knockback
    private float? knockbackStartTime;  // time when knockback started
    private bool isKnockedBack = false;  // flag for knockback state
    private float? lastKnockbackTime;
    private float knockbackCooldown = 1.0f;

    void Start()
    {
        // Initialize GameManager (make sure you've set it up in the Inspector or find it dynamically)
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!lastKnockbackTime.HasValue || Time.time - lastKnockbackTime.Value > knockbackCooldown)
            {
                // Calculate the direction from the obstacle to the player
                Vector3 directionFromCubeToPlayer = player.transform.position - other.transform.position;
                directionFromCubeToPlayer.y = 0;  // Assuming you want to keep knockback horizontal
                directionFromCubeToPlayer.Normalize();  // Make it a unit vector

                KnockbackPlayer(directionFromCubeToPlayer);  // Call the Knockback function here

                lastKnockbackTime = Time.time;

            }
        }
    }

    private void KnockbackPlayer(Vector3 directionFromCubeToPlayer)
    {
        Debug.Log("Knockback called");
        knockbackDirection = directionFromCubeToPlayer.normalized;
        knockbackStartTime = Time.time;
        isKnockedBack = true;
    }
}
