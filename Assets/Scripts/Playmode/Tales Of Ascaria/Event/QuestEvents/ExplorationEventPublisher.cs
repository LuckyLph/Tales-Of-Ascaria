using Harmony;

namespace TalesOfAscaria
{
  public class ExplorationEventPublisher : GameScript
  {
    private ExplorationEventChannel eventChannel;

    private void InjectExplorationEventPublisher([EventChannelScope] ExplorationEventChannel eventChannel)
    {
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectExplorationEventPublisher");
    }

    public void Publish(ExplorationObjective.ExplorationObjectives explorationNodeIndex)
    {
      eventChannel.Publish(new ExplorationEvent(explorationNodeIndex));
    }
  }
}