using Harmony;

namespace TalesOfAscaria
{
  public class InteractionEventPublisher : GameScript
  {
    private InteractionEventChannel eventChannel;

    private void InjectInteractionEventPublisher([EventChannelScope] InteractionEventChannel eventChannel)
    {
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectInteractionEventPublisher");
    }

    public void Publish(InteractionObjective.InteractionObjectives interactorIndex)
    {
      eventChannel.Publish(new InteractionEvent(interactorIndex));
    }
  }
}