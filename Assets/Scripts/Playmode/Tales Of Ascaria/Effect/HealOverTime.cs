using System;
using System.Collections;
using UnityEngine;

namespace TalesOfAscaria 
{
  /// <summary>
  /// Représente un effet de guérison s'effectuant sur une période de temps
  /// </summary>
  [CreateAssetMenu(fileName = "New HealOverTimeEffect", menuName = "Game/Effect/HealOverTime")]
  public class HealOverTime : Effect
  {
    [Tooltip("Le nombre de vie à ajouter au total")] [SerializeField]
    private int totalHeal;

    [Tooltip("Le nombre de tick par seconde. N'affecte pas le healing total")]
    [SerializeField]
    private float ticksPerSecond;

    [Tooltip("La durée totale")]
    [SerializeField]
    private float duration;

    private float timeAtStart;


	  public void SetHeal(int healAmount)
	  {
	    totalHeal = healAmount;
	  }

	  public override void ApplyOn(LivingEntity entity)
	  {
	    timeAtStart = Time.time;
	    entity.StartCoroutine(DoHealOverTime(entity));
	  }

    private IEnumerator DoHealOverTime (LivingEntity entity)
    {
      float timeBetweenTicks = (1f / ticksPerSecond);
      int totalTicks = Mathf.FloorToInt(duration / timeBetweenTicks);
      int healPerTick = Mathf.RoundToInt(totalHeal / totalTicks);

      while (Time.time <= timeAtStart + duration)
      {
        entity.TakeUnreductibleDamage(-1 * healPerTick);
        Debug.Log("I healed for: " + healPerTick);
        yield return new WaitForSeconds(timeBetweenTicks);
      }
    }
	}
}