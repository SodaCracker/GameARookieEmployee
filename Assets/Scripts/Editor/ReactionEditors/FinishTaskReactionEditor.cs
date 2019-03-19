using UnityEditor;

[CustomEditor(typeof(FinishTaskReaction))]
public class FinishTaskReactionEditor : ReactionEditor
{
    protected override string GetFoldoutLabel()
    {
        return "Finish Task Reaction";
    }
}
