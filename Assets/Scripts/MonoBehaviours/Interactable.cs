using UnityEngine;

// 本脚本 attach 在一个有 collider 的物体上，并且该物体还要 attach 一个 event trigger
// event trigger 显示该物体的 caption, 并接受点击，进行 Interact()
public class Interactable : MonoBehaviour
{
    public Transform interactionLocation;
    public ConditionCollection[] conditionCollections = new ConditionCollection[0];
    public ReactionCollection defaultReactionCollection;

    public void Interact()
    {
        for (int i = 0; i < conditionCollections.Length; i++)
        {
            if (conditionCollections[i].CheckAndReact())
                return;
        }

        defaultReactionCollection.React();
    }
}
