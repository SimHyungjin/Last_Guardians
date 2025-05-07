using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationConnect : MonoBehaviour //몬스터 프리팹 애니메이션 컨트롤러
{
    public BaseMonster BaseMonster { get; set; }
    public Animator Animator { get; set; }
    public static bool IsAnimationEventInit { get; private set; } = false;

    public void StartMoveAnimation()
    {
        Animator.SetBool("1_Move", true);
    }

    public void StopMoveAnimation()
    {
        Animator.SetBool("1_Move", false);
    }

    public void StartSturnAnimation()
    {
        Animator.SetBool("1_Move", false);
        Animator.SetBool("5_Debuff", true);
    }

    public void StopSturnAnimation()
    {
        Animator.SetBool("5_Debuff", false);
    }

    public void StartAttackAnimation()
    {
        Animator.SetTrigger("2_Attack");
    }

    public void StartDeathAnimaiton()
    {
        Animator.SetTrigger("4_Death");
    }
    public static void AddAnimationEvent(RuntimeAnimatorController contorller)
    {
        //RuntimeAnimatorController contorller = animator.runtimeAnimatorController;
        foreach (var clip in contorller.animationClips)
        {
            if (clip.name == "ATTACK")
            {
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.time = 0.25f;
                animationEvent.functionName = nameof(Attack);

                clip.AddEvent(animationEvent);
            }

            if (clip.name == "DEATH")
            {
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.time = 0.4f;
                animationEvent.functionName = nameof(Death);

                clip.AddEvent(animationEvent);
            }

        }

    }


    public void Attack()
    {
        BaseMonster.Attack();
    }

    public void Death()
    {
        BaseMonster.Death();
    }
}
