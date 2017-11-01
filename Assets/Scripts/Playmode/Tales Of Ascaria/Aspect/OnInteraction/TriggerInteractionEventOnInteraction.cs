using Harmony;

namespace TalesOfAscaria
{
  public class TriggerInteractionEventOnInteraction : GameScript
  {
    private NPCController npcController;
    private InteractionEventPublisher interactionEventPublisher;
    private DialogueController dialogueController;

    private void InjectTriggerInteractionEventOnInteraction([SiblingsScope] NPCController npcController,
                                                            [SiblingsScope] InteractionEventPublisher interactionEventPublisher,
                                                            [GameObjectScope] DialogueController dialogueController)
    {
      this.npcController = npcController;
      this.interactionEventPublisher = interactionEventPublisher;
      this.dialogueController = dialogueController;
    }

    private void Awake()
    {
      InjectDependencies("InjectTriggerInteractionEventOnInteraction");

      npcController.OnInteractionSensorTriggered += OnInteractionSensorTriggered;
    }

    private void OnDestroy()
    {
      npcController.OnInteractionSensorTriggered -= OnInteractionSensorTriggered;
    }

    private void OnInteractionSensorTriggered(InteractionObjective.InteractionObjectives interactorIndex, XInputDotNetPure.PlayerIndex index)
    {
      if (dialogueController.QuestDialogueFinished)
      {
        interactionEventPublisher.Publish(interactorIndex);
      }
    }
  }
}