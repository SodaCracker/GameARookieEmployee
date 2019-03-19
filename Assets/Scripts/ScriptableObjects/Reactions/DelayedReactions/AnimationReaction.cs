using UnityEngine;

public class AnimationReaction : DelayedReaction
{
    // 需要 connect 到动画控制上的触发器
    public Animator animator;
    // 触发器的 string
    public string trigger;
    // 把 string 换成 hash 表示
    private int triggerHash;

    protected override void SpecificInit()
    {
        triggerHash = Animator.StringToHash(trigger);
    }

    protected override void ImmediateReaction()
    {
        animator.SetTrigger(triggerHash);
    }
}
