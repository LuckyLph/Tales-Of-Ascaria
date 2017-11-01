using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class StaticAnimator : GameScript
  {
    private Animator animator;

    private void InjectStaticAnimator([SiblingsScope] Animator animator)
    {
      this.animator = animator;
    }

    private void Awake()
    {
      InjectDependencies("InjectStaticAnimator");
    }

    public void SetAnimationState(string parameterString, bool state)
    {
      animator.SetBool(parameterString, state);
    }
  }
}