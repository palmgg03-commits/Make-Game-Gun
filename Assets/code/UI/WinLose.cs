using UnityEngine;

public class WinLose : MonoBehaviour
{
    public GameObject winUI;
    public GameObject loseUI;

    public void WinGame()
    {
        Time.timeScale = 0f;
        winUI.SetActive(true);
    }

    public void LoseGame()
    {
        Time.timeScale = 0f;
        loseUI.SetActive(true);
    }
}
