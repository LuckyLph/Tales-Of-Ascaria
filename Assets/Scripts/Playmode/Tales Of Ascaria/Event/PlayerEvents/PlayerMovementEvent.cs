using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class PlayerMovementEvent : IEvent
  {
    public GameObject MovedPlayer { get; private set; }
    public Vector2 MovementVector { get; private set; }

    public PlayerMovementEvent(GameObject movedPlayer, Vector2 movementVector)
    {
      MovedPlayer = movedPlayer;
      MovementVector = movementVector;
    }
  }
}