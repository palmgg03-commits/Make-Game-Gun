using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MainMenu
{
    [Header("UI Panels (Pop-ups)")]
    [SerializeField] private GameObject pausePanel;
    public bool isPaused = false;
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freezes time

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Restores normal time

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
