using UnityEngine;

public class GameObjectReaction : DelayedReaction
{
    public GameObject gameObject;
    // ** gameObject.SetActive(true / false);
    public bool activeState;
    
    protected override void ImmediateReaction()
    {
        gameObject.SetActive(activeState);
    }
}