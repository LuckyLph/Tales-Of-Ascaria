using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ActivateQuestOnCollision : GameScript
  {
    [SerializeField]
    private int questIndexToActivate;

    private PlayerSensor playerSensor;
    private QuestStartedEventPublisher questStartedEventPublisher;

    private void InjectActivateQuestOnTrigger([GameObjectScope] PlayerSensor playerSensor,
                                              [SiblingsScope] QuestStartedEventPublisher questStartedEventPublisher)
    {
      this.playerSensor = playerSensor;
      this.questStartedEventPublisher = questStartedEventPublisher;
    }

    private void Awake()
    {
      InjectDependencies("InjectActivateQuestOnTrigger");

      playerSensor.OnPlayerSensorEntered += OnPlayerSensorTriggered;
    }

    private void OnDestroy()
    {
      playerSensor.OnPlayerSensorEntered -= OnPlayerSensorTriggered;
    }

    private void OnPlayerSensorTriggered(GameObject player)
    {
      ActivateQuest();
      gameObject.SetActive(false);
    }

    public void ActivateQuest()
    {
      questStartedEventPublisher.Publish(questIndexToActivate);
    }
  }
}