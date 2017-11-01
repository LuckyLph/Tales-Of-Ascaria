using Harmony;

namespace TalesOfAscaria
{
  public class InteractOnInput : GameScript
  {
    private InteractionStimulus interactionStimulus;
    private LivingEntity livingEntity;
    private PlayerInput playerInput;

    private void InjectInteractOnInput([EntityScope] InteractionStimulus interactionStimulus,
                                       [GameObjectScope] LivingEntity livingEntity,
                                       [GameObjectScope] PlayerInput playerInput)
    {
      this.interactionStimulus = interactionStimulus;
      this.livingEntity = livingEntity;
      this.playerInput = playerInput;
    }

    private void Awake()
    {
      InjectDependencies("InjectInteractOnInput");
    }

    private void OnEnable()
    {
      playerInput.OnButtonBPressed += Interact;
    }

    private void OnDisable()
    {
      playerInput.OnButtonBPressed -= Interact;
    }

    private void Interact()
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        interactionStimulus.Interact();
      }
    }
  }
}