using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class PlayerInitializedEventPublisher : GameScript
  {
    private PlayerInitializedEventChannel eventChannel;

    private void InjectPlayerInitializedEventPublisher([EventChannelScope] PlayerInitializedEventChannel eventChannel)
    {
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectPlayerInitializedEventPublisher");
    }

    public void Publish(GameObject playerInitialized)
    {
      eventChannel.Publish(new PlayerInitializedEvent(playerInitialized));
    }
  }
}