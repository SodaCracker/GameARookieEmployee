using UnityEngine;
using UnityEngine.UI;

public class TaskText : MonoBehaviour
{
    public const int taskNum = 4;
    public Text taskText;
    public Task[] tasks = new Task[4];

    private string[] strTasks = new string[taskNum];

    private void Start()
    {
        for (int i = 0; i < strTasks.Length; i++)
        {
            strTasks[i] = tasks[i].taskName;
        }

        GenerateText();
    }

    public void FinishTask(Task taskHasFinished)
    {
        int index = GetIndex(taskHasFinished);

        strTasks[index] = "<color=#505050>" + strTasks[index] + "</color>";

        GenerateText();
    }

    private int GetIndex(Task taskHasFinished)
    {
        int taskIndex;

        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i] == taskHasFinished)
            {
                taskIndex = i;
                return i;
            }
        }
        return -1;
    }

    private void GenerateText()
    {
        taskText.text = "";

        for (int i = 0; i < strTasks.Length - 1; i++)
        {
            taskText.text += strTasks[i];
            taskText.text += "\r\n";
        }
        taskText.text += strTasks[strTasks.Length - 1];
    }
}
