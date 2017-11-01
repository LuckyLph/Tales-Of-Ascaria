using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ActivateQuestOnTrigger : GameScript
  {
    [SerializeField]
    private int questToActivateIndex;

    private QuestStartedEventPublisher questStartedEventPublisher;

    private void InjectActivateQuestOnTrigger([EntityScope] QuestStartedEventPublisher questStartedEventPublisher)
    {
      this.questStartedEventPublisher = questStartedEventPublisher;
    }

    private void Awake()
    {
      InjectDependencies("InjectActivateQuestOnTrigger");
    }

    public void ActivateQuest()
    {
      questStartedEventPublisher.Publish(questToActivateIndex);
    }
  }
}