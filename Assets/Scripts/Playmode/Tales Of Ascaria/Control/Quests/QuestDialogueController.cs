using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class QuestDialogueController : GameScript
  {
    [Tooltip("The array of quest dialogues")]
    [SerializeField]
    private string[] questDialogues;

    [Tooltip("The array of dialogues after the quest")]
    [SerializeField]
    private string[] postQuestDialogues;

    [Tooltip("The index of the quest")]
    [SerializeField]
    private int questIndex;

    [Tooltip("Dialogue controller")]
    [SerializeField]
    private DialogueController dialogueController;

    public bool QuestDialogueCompleted { get; set; }
    public bool QuestStarted { get; set; }
    private bool questDone;
    
    private QuestStartedEventChannel questStartedEventChannel;
    private QuestEndedEventChannel questEndedEventChannel;

    private void InjectQuestDialogueController([EventChannelScope] QuestStartedEventChannel questStartedEventChannel,
                                               [EventChannelScope] QuestEndedEventChannel questEndedEventChannel)
    {
      this.questStartedEventChannel = questStartedEventChannel;
      this.questEndedEventChannel = questEndedEventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectQuestDialogueController");

      questStartedEventChannel.OnEventPublished += OnQuestStartedEventPublished;
      questEndedEventChannel.OnEventPublished += OnQuestEndedEventPublished;
    }

    private void Update()
    {
      if (QuestStarted && dialogueController.QuestDialogueFinished)
      {
        QuestDialogueCompleted = true;
      }
      else
      {
        QuestDialogueCompleted = false;
      }
    }

    private void OnQuestStartedEventPublished(QuestStartedEvent startedQuest)
    {
      if (startedQuest.StartedQuestIndex == questIndex && !QuestDialogueCompleted)
      {
        dialogueController.Dialogues = questDialogues;
        QuestStarted = true;
      }
    }

    private void OnQuestEndedEventPublished(QuestEndedEvent endedQuest)
    {
      if (endedQuest.EndedQuestIndex == questIndex && !questDone)
      {
        dialogueController.Dialogues = postQuestDialogues;
      }
    }
  }
}