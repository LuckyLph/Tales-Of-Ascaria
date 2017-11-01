using System.Collections;
using System.Collections.Generic;
using TalesOfAscaria;
using UnityEngine;

namespace TalesOfAscaria
{
  public class HolyBlastController : GameScript
  {
    [Tooltip("Wisdom percentage to use to deal damage")]
    [SerializeField]
    private readonly float holyBlastWisdomPercentage;

    [Tooltip("Effects that the Ki Blast should apply")]
    [SerializeField]
    List<Effect> holyBlastEffects = new List<Effect>();


    public void SetHolyBlastParameters(GameObject holyBlastClone, Vector3 direction, StatsSnapshot playerStats)
    {
      //Ajout des dégats bonus
      InstantDamage holyBlastInstantDamage = holyBlastEffects[0] as InstantDamage;
      holyBlastInstantDamage.BonusDamage = playerStats.Wisdom * holyBlastWisdomPercentage;
      holyBlastEffects[0] = holyBlastInstantDamage;

      //Rotation et direction
      holyBlastClone.transform.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(direction, Vector3.right, Vector3.back));
      holyBlastClone.GetComponentInChildren<ProjectileController>().Direction = direction;

      ActorAction actorAction = new ActorAction(holyBlastEffects);
      holyBlastClone.GetComponentInChildren<HitStimulus>().ActorAction = actorAction;
    }
  }

}

