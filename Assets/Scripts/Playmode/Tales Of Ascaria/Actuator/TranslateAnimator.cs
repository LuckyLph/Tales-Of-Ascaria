using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Object/Actuator/TranslateAnimator")]
  public class TranslateAnimator : GameScript
  {
    private Animator animator;

    private void InjectTranslateAnimator([SiblingsScope] Animator animator)
    {
      this.animator = animator;
    }

    private void Awake()
    {
      InjectDependencies("InjectTranslateAnimator");
    }

    private void Start()
    {
      animator.SetFloat(R.S.AnimatorParameter.InputY, Vector2.down.y);
    }

    public void Animate(Vector2 movementVector)
    {
      animator.SetFloat(R.S.AnimatorParameter.InputX, movementVector.x);
      animator.SetFloat(R.S.AnimatorParameter.InputY, movementVector.y);
      animator.speed = Mathf.Max(Vector2.Distance(Vector2.zero, movementVector), 0.2f);
    }

    public void SetAnimationState(string parameterString, bool state)
    {
      animator.SetBool(parameterString, state);
    }
  }
}