using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float respawnDelay = 2f;
    public GameObject completeLevelUI;
    public GameObject player;
    public Transform spawnPoint;
    public Vector3 checkpoint;
    public bool isCheckpointReached = false;


    private void Update()
    {
        //Debug.Log("Update running");

        // Debug the player's y position
        //Debug.Log("Player's Y Position: " + player.transform.position.y);

        if (player.transform.position.y < 70)
        {
            // Debug to verify this block is entered
            Debug.Log("Player fell below 73, attempting to respawn...");

            if (isCheckpointReached)
            {
                Debug.Log("Checkpoint reached, invoking CheckpointRespawn");
                Invoke("CheckpointRespawn", respawnDelay);
            }
            else
            {
                Debug.Log("No checkpoint, invoking Respawn");
                Invoke("Respawn", respawnDelay);
            }
        }
    }


    public void EndGame()
    {
        if (isCheckpointReached)
        {
            Invoke("CheckpointRespawn", respawnDelay);
        }
        else
        {
            Invoke("Respawn", respawnDelay);
        }

    }

    public void CompleteLevel()
    {
        Invoke("SetActiveDelayed", 1.0f);
        Invoke("FreezeGame", 3.0f);
    }

    // Method to set completeLevelUI active
    private void SetActiveDelayed()
    {
        completeLevelUI.SetActive(true);
    }

    private void FreezeGame()
    {
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Respawn()
    {
        player.transform.position = spawnPoint.position;
        PlayerRagdoll playerRagdoll = player.GetComponent<PlayerRagdoll>();
        if (playerRagdoll != null)
        {
            playerRagdoll.RefreshRagdoll();
        }
    }

    void CheckpointRespawn()
    {
        player.transform.position = checkpoint;
        PlayerRagdoll playerRagdoll = player.GetComponent<PlayerRagdoll>();
        if (playerRagdoll != null)
        {
            playerRagdoll.RefreshRagdoll();
        }
    }
}
