using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class FocusAttackStrikeController : GameScript
  {
    [Tooltip("Effects that the strike should apply")]
    [SerializeField]
    private List<Effect> effects;

    [Tooltip("Amount of strength to use for the strike")]
    [SerializeField]
    private float strengthPercentage;

    private readonly float durationBeforeDestroyed = 0.5f;
    private Collider2D hitbox;
    private HitStimulus hitStimulus;

    private void InjectFocusAttackStrikeController([EntityScope] Collider2D hitbox,
                                                  [EntityScope] HitStimulus hitStimulus)
    {
      this.hitbox = hitbox;
      this.hitStimulus = hitStimulus;
    }

    private void Awake()
    {
      InjectDependencies("InjectFocusAttackStrikeController");
    }

    private void Start()
    {
      hitbox.enabled = false;
      StartCoroutine(DoFocusAttackStrike());
      Destroy(gameObject.transform.root.gameObject, durationBeforeDestroyed);
    }

    public void SetFocusAttackStrikeParameters(GameObject focusAttackStrikeClone, Vector2 direction, StatsSnapshot playerStats, Transform playerPosition)
    {
      Debug.Log("Focus attack striking!");
      for (int i = 0; i < effects.Count; i++)
      {
        if (effects[i].GetType() == typeof(InstantDamage))
        {
          InstantDamage damage = effects[i] as InstantDamage;
          damage.BonusDamage = playerStats.Strength * strengthPercentage;
          effects[i] = damage;
        }
        if (effects[i].GetType() == typeof(Knockback))
        {
          Knockback knockback = effects[i] as Knockback;
          knockback.KnockbackSourceDirection = gameObject.transform;
          effects[i] = knockback;
        }
      }
      focusAttackStrikeClone.transform.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(direction, Vector3.right, Vector3.back));

      ActorAction action = new ActorAction(effects);
      hitStimulus.ActorAction = action;
    }

    private IEnumerator DoFocusAttackStrike()
    {
      hitbox.enabled = true;
      yield return new WaitForSeconds(Time.deltaTime * 2);
      hitbox.enabled = false;
      yield return null;
    }
  }
}

