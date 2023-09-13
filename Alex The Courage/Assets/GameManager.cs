using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
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
        //if (gameHasEnded == false)
        //{
        //    gameHasEnded = true;
        //    Debug.Log("Game Over");
        //    Invoke("Respawn", respawnDelay);
        //}
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
        completeLevelUI.SetActive(true);
    }

    void Restart()
    {
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
