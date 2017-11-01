using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ScreenFader : GameScript
  {
    private Animator animator;
    private WarpTriggerHandler warpTriggerHandler;

    private void InjectScreenFader([EntityScope] Animator animator)
    {
      this.animator = animator;
    }

    private void Awake()
    {
      InjectDependencies("InjectScreenFader");
    }

    public void FadeOutAndIn(WarpTriggerHandler warpTriggerHandler)
    {
      this.warpTriggerHandler = warpTriggerHandler;
      animator.SetTrigger(R.S.AnimatorParameter.FadeOut);
    }

    public void FadeInComplete()
    {
      animator.SetTrigger(R.S.AnimatorParameter.Idle);
    }

    public void FadeOutComplete()
    {
      warpTriggerHandler();
      animator.SetTrigger(R.S.AnimatorParameter.FadeIn);
    }
  }
}