using UnityEngine;

public class BehaviourReaction : DelayedReaction
{
    public Behaviour behaviour;
    // ** 组件.enabled = true / false;
    public bool enabledState;

    protected override void ImmediateReaction()
    {
        behaviour.enabled = enabledState;
    }
}