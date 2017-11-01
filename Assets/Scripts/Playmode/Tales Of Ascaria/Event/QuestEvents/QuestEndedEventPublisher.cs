using Harmony;

namespace TalesOfAscaria
{
  public class QuestEndedEventPublisher : GameScript
  {
    private QuestEndedEventChannel eventChannel;

    private void InjectQuestEndedEventPublisher([EventChannelScope] QuestEndedEventChannel eventChannel)
    {
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectQuestEndedEventPublisher");
    }

    public void Publish(int quest)
    {
      eventChannel.Publish(new QuestEndedEvent(quest));
    }
  }
}