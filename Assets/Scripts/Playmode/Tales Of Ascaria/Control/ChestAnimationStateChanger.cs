using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ChestAnimationStateChanger : GameScript
  {
    private Animator animator;

    public void InjectChestAnimationComplete([GameObjectScope] Animator animator)
    {
      this.animator = animator;
    }

    private void Awake()
    {
      InjectDependencies("InjectChestAnimationComplete");
    }

    public void ChestAnimationComplete()
    {
      animator.SetBool(R.S.AnimatorParameter.IsOpen, true);
    }
  }
}