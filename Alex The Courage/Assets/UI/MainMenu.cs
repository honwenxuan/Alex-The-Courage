using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToLevel2()
    {
        SceneManager.LoadScene(2);
    }

    public void GoToLevel3()
    {
        SceneManager.LoadScene(3);
    }

    public void GoToLevel4()
    {
        SceneManager.LoadScene(4);
    }

    public void GoToLevel5()
    {
        SceneManager.LoadScene(5);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
