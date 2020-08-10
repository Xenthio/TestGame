using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("main");
    }
    public void quitGame()
    {
        Application.Quit();
    }
}