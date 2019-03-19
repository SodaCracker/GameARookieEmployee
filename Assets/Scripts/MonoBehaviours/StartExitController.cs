using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartExitController : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;

    public void StartGame()
    {
        SceneManager.LoadScene("Persistent");
    }

    public void ExitGame()
    {
        Debug.Log("Bye");
        Application.Quit();
    }
}
