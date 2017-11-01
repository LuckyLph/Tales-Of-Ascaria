using System;
using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria 
{
	public class FanOfBladesController : GameScript 
	{
	  [Tooltip("Effets des couteaux lancés par le spell")]
	  [SerializeField] List<Effect> fanOfBladesEffects = new List<Effect>();

	  [Tooltip("Pourcentage de strength de l'entité appliqué aux couteaux pour les dommages")]
	  [SerializeField] private float knifePower;

	  [Tooltip("Angle entre le premier et le dernier couteau")]
    [SerializeField] private float knifeThrowAngle;

	  [Tooltip("Nombre de couteaux lancés")]
	  [SerializeField] private int knifeCount;

    [Tooltip("Préfab d'un couteau lancé")]
	  [SerializeField] private GameObject knife;

	  [Tooltip("Temps avant que les couteaux soient lancés")]
	  [SerializeField] private float fanOfBladesCastTime;

    public IEnumerator SetFanOfBladesParameters(StatsSnapshot playerStats, Vector3 direction, Vector3 playerPosition)
    {
      yield return new WaitForSeconds(fanOfBladesCastTime);
	    for (int i = 0; i < fanOfBladesEffects.Count; i++)
	    {
	      if (fanOfBladesEffects[i] is InstantDamage)
	      {
	        InstantDamage knifeInstantDamage = fanOfBladesEffects[i] as InstantDamage;
	        knifeInstantDamage.BonusDamage = playerStats.Strength * knifePower;
	        fanOfBladesEffects[i] = knifeInstantDamage;
	      }
	    }
	    ActorAction actorAction = new ActorAction(fanOfBladesEffects);
      for (int i = 0; i < knifeCount; i++)
	    {
        float knifeAngle = -knifeThrowAngle / 2 + i * knifeThrowAngle / (knifeCount - 1);
	      float directionInFloatAngle = Mathf.Rad2Deg * (Mathf.Atan2(direction.y, direction.x)) + knifeAngle;
        Vector3 knifeRotation = new Vector3(0,0,directionInFloatAngle);
        Vector3 knifeDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * directionInFloatAngle),
                                             Mathf.Sin(Mathf.Deg2Rad *directionInFloatAngle));
        GameObject knifeClone = Instantiate(knife, playerPosition, Quaternion.identity);
        knifeClone.transform.Rotate(knifeRotation);
        knifeClone.GetComponentInChildren<HitStimulus>().ActorAction = actorAction;
	      knifeClone.GetComponentInChildren<ProjectileController>().Direction = knifeDirection;
      }
      Destroy(transform.root.gameObject);
	  }
  }
}