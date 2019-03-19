using UnityEngine;

public class AllConditions : ResettableScriptableObject
{
    // 保存了本游戏当前所有 conditions
    public Condition[] conditions;
    private const string loadPath = "AllConditions";

    // 单例模式
    private static AllConditions instance;

    public static AllConditions Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<AllConditions>();
            if (!instance)
                instance = Resources.Load<AllConditions>(loadPath);
            if (!instance)
                Debug.Log("AllConditions 尚未创建，请先创建 \"AllCondition\".");
            return instance;
        }
        set { instance = value; }
    }

    public override void Reset()
    {
        if (conditions == null)
            return;

        for (int i = 0; i < conditions.Length; i++)
        {
            conditions[i].satisfied = false;
        }
    }

    public static bool CheckCondition(Condition requiredCondition)
    {
        // 当前已保存在 assets 中的所有 condition(s)
        Condition[] allConditions = Instance.conditions;
        Condition globalCondition = null;

        if (allConditions != null & allConditions[0] != null)
        {
            for (int i = 0; i < allConditions.Length; i++)
            {
                if (allConditions[i].hash == requiredCondition.hash)
                    globalCondition = allConditions[i];
            }
        }

        if (!globalCondition)
            return false;

        return globalCondition.satisfied == requiredCondition.satisfied;
    }
}

