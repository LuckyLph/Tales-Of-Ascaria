using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class PlayerRespawnEvent : IEvent
  {
    public GameObject[] RespawnedPlayers { get; set; }

    public PlayerRespawnEvent(GameObject[] respawnedPlayers)
    {
      RespawnedPlayers = respawnedPlayers;
    }
  }
}