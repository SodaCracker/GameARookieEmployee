public class FinishTaskReaction : DelayedReaction
{
    public Task task;
    private TaskText taskText;

    protected override void SpecificInit()
    {
        taskText = FindObjectOfType<TaskText>();
    }

    protected override void ImmediateReaction()
    {
        taskText.FinishTask(task);
    }
}
