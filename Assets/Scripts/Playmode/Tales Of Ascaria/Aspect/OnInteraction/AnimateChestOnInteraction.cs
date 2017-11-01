using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class AnimateChestOnInteraction : GameScript
  {
    private Animator animator;
    private InteractionSensor interactionSensor;

    private void InjectAnimateChestOnInteraction([SiblingsScope] Animator animator,
                                                 [SiblingsScope] InteractionSensor interactionSensor)
    {
      this.animator = animator;
      this.interactionSensor = interactionSensor;
    }

    private void Awake()
    {
      InjectDependencies("InjectAnimateChestOnInteraction");
    }

    private void OnEnable()
    {
      interactionSensor.OnInteractionTrigger += AnimateChest;
    }

    private void OnDisable()
    {
      interactionSensor.OnInteractionTrigger -= AnimateChest;
    }

    private void AnimateChest(XInputDotNetPure.PlayerIndex playerIndex)
    {
      animator.SetTrigger(R.S.AnimatorParameter.MustOpen);
    }
  }
}