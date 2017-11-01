using Harmony;

namespace TalesOfAscaria
{
  public class CameraSizeChangedEventPublisher : GameScript
  {
    private CameraSizeChangedEventChannel eventChannel;

    private void InjectCameraSizeChangedEventPublisher([EventChannelScope] CameraSizeChangedEventChannel eventChannel)
    {
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectCameraSizeChangedEventPublisher");
    }

    public void Publish(float size, float aspect)
    {
      eventChannel.Publish(new CameraSizeChangedEvent(size, aspect));
    }
  }
}