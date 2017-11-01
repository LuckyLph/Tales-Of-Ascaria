using Harmony;

namespace TalesOfAscaria
{
  public class QuestStartedEventPublisher : GameScript
  {
    private QuestStartedEventChannel eventChannel;

    private void InjectQuestStartedEventPublisher([EventChannelScope] QuestStartedEventChannel eventChannel)
    {
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectQuestStartedEventPublisher");
    }

    public void Publish(int quest)
    {
      eventChannel.Publish(new QuestStartedEvent(quest));
    }
  }
}