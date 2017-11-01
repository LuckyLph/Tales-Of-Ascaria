using System;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void HitStimulusEventHandler(ActorAction actorAction);

  [AddComponentMenu("Game/World/Object/Stimulus/HitStimulus")]
  public class HitStimulus : GameScript
  {
    private new Collider2D collider2D;

    public virtual event HitStimulusEventHandler OnHit;

    public ActorAction ActorAction { get; set; }

    public void InjectHitStimulus([GameObjectScope] Collider2D collider2D)
    {
      this.collider2D = collider2D;
    }

    public void Awake()
    {
      InjectDependencies("InjectHitStimulus");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
      HitSensor hitSensor = other.GetComponent<HitSensor>();
      if (hitSensor != null)
      {
        hitSensor.Hit(ActorAction);
        if (OnHit != null) OnHit(ActorAction);
      }
    }
  }
}