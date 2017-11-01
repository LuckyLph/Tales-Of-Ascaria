using System.Collections;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{

  /// <summary>
  /// Immunise contre tout effet négatifs à l'entité
  /// </summary>
  [CreateAssetMenu(fileName = "New Debuff Immunity Effect", menuName = "Game/Effect/DebuffImmunity")]
  public class DebuffImmunity : Effect
	{

    [SerializeField]
    [Tooltip("Le nombre de seconde que l'immunité durera")]
	  public float duration;

	  private float endTime;

	  public override void ApplyOn(LivingEntity entity)
	  {
	    endTime = Time.time + duration;
	    entity.StartCoroutine(RepeatCleanse(entity));
	  }

	  private IEnumerator RepeatCleanse(LivingEntity entity)
	  {
	    while (Time.time < endTime)
	    {
	      entity.Cleanse();
	      yield return null;
	    }
	  }
	}
}