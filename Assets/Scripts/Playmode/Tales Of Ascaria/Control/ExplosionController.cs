using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Harmony;

namespace TalesOfAscaria
{
  public class ExplosionController : GameScript
  {
    [Tooltip("Effects that the explosion should apply")]
    [SerializeField]
    private List<Effect> effects;

    [Tooltip("Amount of wisdom to use for the explosion")]
    [SerializeField]
    private float wisdomPercentage;

    private HitStimulus hitStimulus;
    private new Collider2D collider;

    private void InjectExplosionController([SiblingsScope] HitStimulus hitStimulus,
                                           [SiblingsScope] Collider2D collider)
    {
      this.hitStimulus = hitStimulus;
      this.collider = collider;
    }

    private void Awake()
    {
      InjectDependencies("InjectExplosionController");
      collider.enabled = false;
    }

    private void Start()
    {
      StartCoroutine(DoExplosion());
    }

    public IEnumerator DoExplosion()
    {
      collider.enabled = true;
      yield return new WaitForSeconds(Time.deltaTime * 2);
      collider.enabled = false;
    }

    public void SetExplosionParameters(StatsSnapshot mobStats)
    {
      for (int i = 0; i < effects.Count; i++)
      {
        if (effects[i] is InstantDamage)
        {
          InstantDamage explosionDamage = effects[i] as InstantDamage;
          explosionDamage.BonusDamage = mobStats.Wisdom * wisdomPercentage;
          effects[i] = explosionDamage;
        }
      }
      ActorAction actorAction = new ActorAction(effects);
      hitStimulus.ActorAction = actorAction;
    }
  }
}