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

        // respawn when player fall down
        if (player.transform.position.y < -10)
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
