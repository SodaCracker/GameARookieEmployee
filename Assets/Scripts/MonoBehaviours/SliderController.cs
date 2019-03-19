using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider timerSlider;
    public Text loseText;

    public float timeToWin;
    private float timeLeft;

    //private void Awake()
    //{
    //    StartCoroutine(StartTimer());
    //}

    private void Start()
    {
        timerSlider.enabled = true;
        timeToWin = 10f;
        timeLeft = timeToWin;
    }

    //private IEnumerator StartTimer()
    //{
    //    yield return new WaitForSeconds(2f);
    //}

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        timerSlider.value = timeLeft / timeToWin;

        if (Mathf.Approximately(timeLeft / timeToWin, 0f))
        {
            loseText.enabled = true;
            Time.timeScale = 0;
            //SceneManager.LoadScene("Start");
        }
    }
}
