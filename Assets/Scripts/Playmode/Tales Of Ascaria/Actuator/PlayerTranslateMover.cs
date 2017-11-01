using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Object/Actuator/PlayerTranslateMover")]
  public class PlayerTranslateMover : TranslateMover
  {
    public bool Move(Vector2 movementVector, float speed, bool canMoveUp, bool canMoveDown, bool canMoveRight, bool canMoveLeft)
    {
      if ((movementVector.x > 0 && !canMoveRight) || (movementVector.x < 0 && !canMoveLeft))
      {
        movementVector.x = 0;
      }
      if ((movementVector.y > 0 && !canMoveUp) || (movementVector.y < 0 && !canMoveDown))
      {
        movementVector.y = 0;
      }
      rigidbody.MovePosition(rigidbody.position + movementVector * speed * Time.deltaTime);
      return movementVector != Vector2.zero;
    }
  }
}