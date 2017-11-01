using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class PlayerWarpEventPublisher : GameScript
  {
    private PlayerWarpEventChannel eventChannel;

    private void InjectPlayerWarpEventChannel([EventChannelScope] PlayerWarpEventChannel eventChannel)
    {
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectPlayerWarpEventChannel");
    }

    public void Publish(GameObject[] warpedPlayers, GameObject warpOrigin, GameObject warpTarget)
    {
      eventChannel.Publish(new PlayerWarpEvent(warpedPlayers, warpOrigin, warpTarget));
    }
  }
}