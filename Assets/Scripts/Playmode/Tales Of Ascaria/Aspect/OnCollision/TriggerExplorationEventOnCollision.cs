using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class TriggerExplorationEventOnCollision : GameScript
  {
    private PlayerSensor playerSensor;
    private ExplorationEventPublisher explorationEventPublisher;
    private ExplorationNode explorationNode;

    private void InjectTriggerExplorationEventOnCollision([GameObjectScope] PlayerSensor playerSensor,
                                                         [GameObjectScope] ExplorationNode explorationNode,
                                                         [EntityScope] ExplorationEventPublisher explorationEventPublisher)
    {
      this.playerSensor = playerSensor;
      this.explorationNode = explorationNode;
      this.explorationEventPublisher = explorationEventPublisher;
    }

    private void Awake()
    {
      InjectDependencies("InjectTriggerExplorationEventOnCollision");

      playerSensor.OnPlayerSensorEntered += OnPlayerSensorTriggered;
    }

    private void OnDestroy()
    {
      playerSensor.OnPlayerSensorEntered -= OnPlayerSensorTriggered;
    }

    private void OnPlayerSensorTriggered(GameObject player)
    {
      explorationEventPublisher.Publish(explorationNode.ID);
    }
  }
}