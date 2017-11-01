using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class PlayerInitializedEvent : IEvent
  {
    public GameObject PlayerInitialized { get; private set;}

    public PlayerInitializedEvent(GameObject playerInitialized)
    {
      PlayerInitialized = playerInitialized;
    }
  }
}