using Harmony;

namespace TalesOfAscaria
{
  public class TriggerDialogueOnInteraction : GameScript
  {
    private DialogueController dialogueController;

    private void InjectTriggerDialogueOnInteraction([GameObjectScope] DialogueController dialogueController)
    {
      this.dialogueController = dialogueController;
    }

    private void Awake()
    {
      InjectDependencies("InjectTriggerDialogueOnInteraction");
    }

    private void OnEnable()
    {
      dialogueController.OnDialogueInteractionTriggered += OnInteractionSensorTriggered;
    }

    private void OnDisable()
    {
      dialogueController.OnDialogueInteractionTriggered -= OnInteractionSensorTriggered;
    }

    private void OnInteractionSensorTriggered(XInputDotNetPure.PlayerIndex playerIndex)
    {
      dialogueController.BeginDialogue();
      if (dialogueController.IndexDialogue < dialogueController.Dialogues.Length)
      {
        dialogueController.DialogueText.text = dialogueController.Dialogues[dialogueController.IndexDialogue];
        if (dialogueController.IndexDialogue == dialogueController.Dialogues.Length - 1)
        {
          dialogueController.QuestDialogueFinished = true;
        }
        dialogueController.IndexDialogue++;
      }
      else
      {
        dialogueController.HideBox();
      }
    }
  }
}