using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public float charsPerSecond = 0.2f;
    private string words;

    private bool isActive = false;
    private float timer;
    private Text myText;
    private int currentPos = 0;

    void Start()
    {
        timer = 0;
        isActive = true;
        myText = GetComponent<Text>();
        words = myText.text;
        myText.text = "";
    }

    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= charsPerSecond)
            {
                timer = 0;
                currentPos++;
                myText.text = words.Substring(0, currentPos);

                if (currentPos >= words.Length)
                    OnFinish();
            }
        }
    }

    public void StartEffect()
    {
        isActive = true;
    }

    private void OnFinish()
    {
        isActive = false;
        timer = 0;
        currentPos = 0;
        myText.text = words;
    }
}
