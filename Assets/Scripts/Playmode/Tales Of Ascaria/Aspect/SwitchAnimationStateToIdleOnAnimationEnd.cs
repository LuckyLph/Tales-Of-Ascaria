using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class SwitchAnimationStateToIdleOnAnimationEnd : GameScript
  {
    private Animator animator;

    private void InjectSwitchAnimationStateToIdleOnAnimationEnd([GameObjectScope] Animator animator)
    {
      this.animator = animator;
    }

    private void Awake()
    {
      InjectDependencies("InjectSwitchAnimationStateToIdleOnAnimationEnd");
    }

    private void ChangeAnimatorStateToIdle()
    {
      animator.SetBool(R.S.AnimatorParameter.IsAttacking, false);
    }
  }
}