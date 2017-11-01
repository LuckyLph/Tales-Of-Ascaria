using UnityEngine;

namespace TalesOfAscaria 
{
  public class RotatedTranslateMover : TranslateMover
  {
    public override void Move(Vector2 movement, float speed)
    {
      transform.root.Translate(Vector3.up * speed * Time.deltaTime);
    }
  }
}
