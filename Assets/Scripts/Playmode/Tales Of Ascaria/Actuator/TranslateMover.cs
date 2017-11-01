using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Object/Actuator/TranslateMover")]
  public class TranslateMover : GameScript
  {
    protected new Rigidbody2D rigidbody;

    private void InjectTranslateMover([ParentScope] Rigidbody2D rigidbody)
    {
      this.rigidbody = rigidbody;
    }

    protected virtual void Awake()
    {
      InjectDependencies("InjectTranslateMover");
    }

    public virtual void Move(Vector2 movement, float speed)
    {
      rigidbody.MovePosition(rigidbody.position + movement * speed * Time.deltaTime);
    }
  }
}