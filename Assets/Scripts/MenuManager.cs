using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loadingScreen;
    public static bool gamePaused;

    public void PlayButton()
    {
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1;
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        gamePaused = true;
    }

    public void ResumeButton()
    {
        Time.timeScale = Player.instance.timeScaleValue;
        gamePaused = false;
    }

    public void HomeButton()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartButton()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RespawnButton()
    {
        Player.instance.RespwanPos();
        gamePaused = false;
    }
}
