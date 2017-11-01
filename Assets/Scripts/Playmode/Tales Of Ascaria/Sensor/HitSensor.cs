using System;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void HitSensorEventHandler(ActorAction action);

  [AddComponentMenu("Game/World/Object/Sensor/HitSensor")]
  public class HitSensor : GameScript
  {
    private new Collider2D collider2D;

    private ActorActionApplier applier;

    public virtual event HitSensorEventHandler OnHit;

    public void InjectHitSensor([GameObjectScope] Collider2D collider2D,
                                [SiblingsScope] ActorActionApplier applier)
    {
      this.collider2D = collider2D;
      this.applier = applier;
    }

    public void Awake()
    {
      InjectDependencies("InjectHitSensor");

      OnHit += applier.OnHit;
    }

    public void OnDestroy()
    {
      OnHit -= applier.OnHit;
    }

    public virtual void Hit(ActorAction actorAction)
    {
      if (actorAction != null)
      {
        applier.OnHit(actorAction);
      }
      else
      {
        Debug.Log("WTF you're null!");
      }
    }
  }
}