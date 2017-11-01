using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class PlayerRespawnEventPublisher : GameScript
  {
    private PlayerRespawnEventChannel eventChannel;

    private void InjectPlayerRespawnEventPublisher([EventChannelScope] PlayerRespawnEventChannel eventChannel)
    {
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectPlayerRespawnEventPublisher");
    }

    public void Publish(GameObject[] respawnedPlayers)
    {
      eventChannel.Publish(new PlayerRespawnEvent(respawnedPlayers));
    }
  }
}