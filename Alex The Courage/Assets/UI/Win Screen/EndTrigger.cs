using System.Collections;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public bool isFinished;
    public Animator animator;
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator = other.GetComponent<Animator>();
            if (animator != null)
            {
                gameManager.CompleteLevel();
                animator.SetBool("IsFinished", true);
                // Unlock the cursor and make it visible
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // Start the coroutine to reset the parameter after 3 seconds
                StartCoroutine(ResetIsFinishedAfterDelay());
            }
            else
            {
                Debug.LogError("Animator not found on the player!");
            }
        }
    }

    IEnumerator ResetIsFinishedAfterDelay()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Set IsFinished back to false
        animator.SetBool("IsFinished", false);
    }
}