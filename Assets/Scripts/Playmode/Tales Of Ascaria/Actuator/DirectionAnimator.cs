using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class DirectionAnimator : GameScript
  {
    private Animator animator;

    private void InjectDirectionAnimator([SiblingsScope] Animator animator)
    {
      this.animator = animator;
    }

    private void Awake()
    {
      InjectDependencies("InjectDirectionAnimator");
    }

    public void SetDirection(Vector2 direction)
    {
      animator.SetFloat(R.S.AnimatorParameter.InputX, direction.x);
      animator.SetFloat(R.S.AnimatorParameter.InputY, direction.y);
    }
  }
}