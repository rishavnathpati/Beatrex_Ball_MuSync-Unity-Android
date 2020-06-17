using TMPro;
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
    public TextMeshProUGUI tipsText;
    private int choice;

    public void PlayButton()
    {
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1;
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        choice = Random.Range(1, 7);
        switch (choice)
        {
            case 1:
                tipsText.text = "TIPS: You get 5 free respawns";
                break;
            case 2:
                tipsText.text = "TIPS: Collect 10 orbs to get a boost jump ";
                break;
            case 3:
                tipsText.text = "TIPS: Enter the vortex for a different experience";
                break;
            case 4:
                tipsText.text = "TIPS: You cannot respawn if you fall down";
                break;
            case 5:
                tipsText.text = "TIPS: Be careful of the Spikey platforms";
                break;
            case 6:
                tipsText.text = "TIPS: Enjoy the seamless experience";
                break;
        }
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

    public void AudioControl(bool vol)
    {
        if (!vol)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

}
