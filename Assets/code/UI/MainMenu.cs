using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels (Pop-ups)")]
    [SerializeField] private GameObject settingsPanel;
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
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
        SceneManager.LoadScene("");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
