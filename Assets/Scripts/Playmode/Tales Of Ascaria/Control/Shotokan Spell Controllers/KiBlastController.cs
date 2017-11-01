using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class KiBlastController : GameScript
  {
    [Tooltip("Wisdom percentage to use to deal damage")]
    [SerializeField]
    private readonly float kiBlastWisdomPercentage;

    [Tooltip("Effects that the Ki Blast should apply")]
    [SerializeField]
    List<Effect> kiBlastEffects = new List<Effect>();


    public void SetKiBlastParameters(GameObject kiBlastClone, Vector3 direction, StatsSnapshot playerStats)
    {
      //Ajout des dégats bonus
      InstantDamage kiBlastInstantDamage = kiBlastEffects[0] as InstantDamage;
      kiBlastInstantDamage.BonusDamage = playerStats.Wisdom * kiBlastWisdomPercentage;
      kiBlastEffects[0] = kiBlastInstantDamage;

      //Rotation et direction
      kiBlastClone.transform.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(direction, Vector3.right, Vector3.back));
      kiBlastClone.GetComponentInChildren<ProjectileController>().Direction = direction;

      ActorAction actorAction = new ActorAction(kiBlastEffects);
      kiBlastClone.GetComponentInChildren<HitStimulus>().ActorAction = actorAction;
    }
  }
}

