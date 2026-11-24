using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels (Pop-ups)")]
    [SerializeField] private GameObject settingsPanel;
    public int nextLevelIndex;
    public Mission mission;

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Map1");
    }
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }
    public void CloseAllPanels()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    public void NextLevel()
    {
        Time.timeScale = 1f;
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            Time.timeScale = 0f;
            mission.StartCoroutine(mission.TimerEnter("There is no next level!"));
        }
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
