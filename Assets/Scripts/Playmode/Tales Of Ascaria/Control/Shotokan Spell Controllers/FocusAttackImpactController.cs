using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class FocusAttackImpactController : GameScript
  {
    [Tooltip("Effects that the strike should apply")]
    [SerializeField]
    private List<Effect> effects;

    [Tooltip("Amount of wisdom to use for the impact")]
    [SerializeField]
    private float wisdomPercentage;

    private readonly float durationBeforeDestroyed = 0.5f;
    private Collider2D hitbox;
    private HitStimulus hitStimulus;

    private void InjectFocusAttackImpactController([EntityScope] Collider2D hitbox,
                                                   [EntityScope] HitStimulus hitStimulus)
    {
      this.hitbox = hitbox;
      this.hitStimulus = hitStimulus;
    }

    private void Awake()
    {
      InjectDependencies("InjectFocusAttackImpactController");
    }

    private void Start()
    {
      hitbox.enabled = false;
      StartCoroutine(DoFocusAttackImpact());
      Destroy(gameObject.transform.root.gameObject, durationBeforeDestroyed);
    }

    public void SetFocusAttackImpactParameters(GameObject focusAttackStrikeClone, Vector2 direction, StatsSnapshot playerStats)
    {
      Debug.Log("Focus attack impacting!");
      for (int i = 0; i < effects.Count; i++)
      {
        if (effects[i].GetType() == typeof(InstantDamage))
        {
          InstantDamage damage = effects[i] as InstantDamage;
          damage.BonusDamage = playerStats.Wisdom * wisdomPercentage;
          effects[i] = damage;
        }
        if (effects[i].GetType() == typeof(Knockback))
        {
          Knockback knockback = effects[i] as Knockback;
          knockback.KnockbackSourceDirection = gameObject.transform;
          effects[i] = knockback;
        }
      }
      ActorAction action = new ActorAction(effects);
      hitStimulus.ActorAction = action;
    }

    private IEnumerator DoFocusAttackImpact()
    {
      hitbox.enabled = true;
      yield return new WaitForSeconds(Time.deltaTime * 2);
      hitbox.enabled = false;
      yield return null;
    }
  }
}

