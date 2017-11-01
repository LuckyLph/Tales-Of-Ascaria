using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class PlayerWarpEvent : IEvent
  {
    public GameObject[] WarpedPlayers { get; private set; }
    public GameObject WarpOrigin { get; private set; }
    public GameObject WarpTarget { get; private set; }

    public PlayerWarpEvent(GameObject[] warpedPlayers, GameObject warpOrigin, GameObject warpTarget)
    {
      WarpedPlayers = warpedPlayers;
      WarpOrigin = warpOrigin;
      WarpTarget = warpTarget;
    }
  }
}