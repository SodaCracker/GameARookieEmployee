using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private  GameObject pauseInterface;
    public GameObject inventory;

    public void Resume()
    {
        Time.timeScale = 1;
        pauseInterface = GameObject.Find("PauseInterface");
        pauseInterface.SetActive(false);
        inventory.SetActive(true);
    }
}
