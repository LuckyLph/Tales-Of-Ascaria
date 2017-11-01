using System.Collections.Generic;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ZenHurricaneController : GameScript
  {
    [Tooltip("Percentage of Wisdom to apply as bonus damage, from 0 to 1.")]
    [SerializeField] private float wisdomPercentage;

    [Tooltip("Effects that the Zen Hurricane's hit should apply. First is the damage, second is the knockback, third is the hitstun.")]
    [SerializeField] private List<Effect> effects;

    [Tooltip("Son joué lorsque le shotokan cast zen hurricane")]
    [SerializeField] private AudioClip zenHurricaneCastSound;

    public void SetZenHurricaneParameters(GameObject zenHurricaneClone, StatsSnapshot playerStats, Transform playerPosition)
    {
      Debug.Log("Zen hurricane set!");
      for (int i = 0; i < effects.Count; i++)
      {
        if (effects[i].GetType() == typeof(InstantDamage))
        {
          InstantDamage instantDamage = effects[i] as InstantDamage;
          instantDamage.BonusDamage = playerStats.Wisdom * wisdomPercentage;
          effects[i] = instantDamage;
        }
        else if (effects[i].GetType() == typeof(Knockback))
        {
          Knockback knockback = effects[i] as Knockback;
          knockback.KnockbackSourceDirection = playerPosition;
          effects[i] = knockback;
        }
      }
      ActorAction actorAction = new ActorAction(effects);
      zenHurricaneClone.GetComponentInChildren<HitStimulus>().ActorAction = actorAction;
    }
  }
}

