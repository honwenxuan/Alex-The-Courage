using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;
    public GameObject pauseMenuUI;
    public GameObject optionMenu;
    public GameObject levelCompleteUI;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when the game scene starts
        Cursor.visible = false; // Hide the cursor when the game scene starts
        GameIsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOptionMenuOpen() && !IsLevelCompleteUIOpen() && Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        else if (IsOptionMenuOpen() && Input.GetKeyDown(KeyCode.Escape))
        {
            optionMenu.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Lock the cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        // Before loading the menu, make sure to set the time scale back to normal
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

        // You may also want to unlock and show the cursor when going back to the menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    bool IsOptionMenuOpen()
    {
        if (optionMenu != null && optionMenu.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsLevelCompleteUIOpen()
    {
        if (levelCompleteUI != null && levelCompleteUI.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
