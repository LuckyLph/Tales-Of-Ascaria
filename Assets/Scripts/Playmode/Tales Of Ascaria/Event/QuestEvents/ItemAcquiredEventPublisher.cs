using Harmony;

namespace TalesOfAscaria
{
  public class ItemAcquiredEventPublisher : GameScript
  {
    private ItemAcquiredEventChannel eventChannel;

    private void InjectItemAcquiredEventPublisher([EventChannelScope] ItemAcquiredEventChannel eventChannel)
    {
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectItemAcquiredEventPublisher");
    }

    public void Publish(Item item)
    {
      eventChannel.Publish(new ItemAcquiredEvent(item));
    }
  }
}