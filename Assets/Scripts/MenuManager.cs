using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loadingScreen;
    public static bool gamePaused;
    public GameObject GetaLife;
    public GameObject GameOverPanel;
    public Text LifeCount;
    public AudioSource respawning321;

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
        if (PlayerPrefs.GetInt("livesRemaining") > 0)
        {
            respawning321.Play();
            gamePaused = true;
            Time.timeScale = 1f;
            Player.instance.RespawnPlayer();//Working     
        }
        else
        {
            GetaLife.SetActive(true);
            GameOverPanel.SetActive(false);
        }
    }

    public void GetALife()
    {
        PlayerPrefs.SetInt("livesRemaining", PlayerPrefs.GetInt("livesRemaining") + 1);
        LifeCount.text = PlayerPrefs.GetInt("livesRemaining").ToString();
    }

    public void LoFiIsTrue()
    {
        PlayerPrefs.SetInt("loFiIs", 1);
        PlayerPrefs.SetInt("EDMis", 0);
        Debug.Log("EDM is: " + PlayerPrefs.GetInt("EDMis") + "Lofi is: " + PlayerPrefs.GetInt("loFiIs"));
    }

    public void EDMIsTrue()
    {
        PlayerPrefs.SetInt("loFiIs", 0);
        PlayerPrefs.SetInt("EDMis", 1);
        Debug.Log("EDM is: " + PlayerPrefs.GetInt("EDMis") + "Lofi is: " + PlayerPrefs.GetInt("loFiIs"));
    }
}
