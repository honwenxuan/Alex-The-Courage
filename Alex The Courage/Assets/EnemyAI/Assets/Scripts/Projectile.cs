
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bulletSpeed = 2.0f;
    public float destroyTime = 2.0f; // Adjust this value to control the time before destruction

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the projectile collides with something
        // For example, you can check for a "Player" tag or a specific layer

        if (other.CompareTag("Player"))
        {
            // Handle damage to the player (you can modify this as needed)
            //PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            //if (playerHealth != null)
            //{
                //playerHealth.TakeDamage(damage);
            //}

            // Destroy the projectile upon collision
            Destroy(gameObject);
        }
    }
}
