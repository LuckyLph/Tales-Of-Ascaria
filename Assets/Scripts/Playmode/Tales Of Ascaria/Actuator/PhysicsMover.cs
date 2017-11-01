using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Object/Actuator/PhysicsMover")]
  public class PhysicsMover : GameScript
  {
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;

    private Transform topParentTransform;
    private Rigidbody2D topParentRigidbody2D;

    private void InjectPhysicsMover([SiblingsScope] Transform topParentTransform,
                                    [SiblingsScope] Rigidbody2D topParentRigidbody2D)
    {
      this.topParentTransform = topParentTransform;
      this.topParentRigidbody2D = topParentRigidbody2D;
    }

    private void Awake()
    {
      InjectDependencies("InjectPhysicsMover");
    }

    public virtual void AddFowardImpulse(float force = 1)
    {
      AddImpulse(topParentTransform.up, force);
    }

    public virtual void AddBackwardImpulse(float force = 1)
    {
      AddImpulse(-topParentTransform.up, force);
    }

    public virtual void AddRotateLeftImpulse(float force = 1)
    {
      AddRotationImpulse(1, force);
    }

    public virtual void AddRotateRightImpulse(float force = 1)
    {
      AddRotationImpulse(-1, force);
    }

    public virtual void AddImpulse(Vector3 direction, float force = 1)
    {
      topParentRigidbody2D.AddForce(direction.normalized * speed * force * Time.deltaTime);
    }

    public virtual void AddImpulseToward(Vector3 destination, float force = 1)
    {
      AddImpulse(destination - topParentTransform.position, force);
    }

    private void AddRotationImpulse(float direction, float force = 1)
    {
      topParentRigidbody2D.AddTorque(direction * rotationSpeed * force * Time.deltaTime);
    }
  }
}