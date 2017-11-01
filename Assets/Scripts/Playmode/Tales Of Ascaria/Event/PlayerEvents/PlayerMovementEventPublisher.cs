using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class PlayerMovementEventPublisher : GameScript
  {
    private PlayerMovementEventChannel eventChannel;

    private void InjectPlayerMovementEventPublisher([EventChannelScope] PlayerMovementEventChannel eventChannel)
    {
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectPlayerMovementEventPublisher");
    }

    public void Publish(GameObject player, Vector2 movementVector)
    {
      eventChannel.Publish(new PlayerMovementEvent(player, movementVector));
    }
  }
}