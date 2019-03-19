using UnityEngine;

public class ConditionCollection : ScriptableObject
{
    public string description;
    public Condition[] requiredConditions = new Condition[0];
    public ReactionCollection reactionCollection;

    public bool CheckAndReact()
    {
        for (int i = 0; i < requiredConditions.Length; i++)
        {
            if (!AllConditions.CheckCondition(requiredConditions[i]))
                return false;
        }

        // 所有 conditons 的 check 都通过
        if (reactionCollection)
            reactionCollection.React();

        return true;
    }
}

