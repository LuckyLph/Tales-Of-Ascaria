using System.Collections;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
  /// <summary>
  /// Immunise contre les effets qui entravent le mouvement
  /// </summary>
  [CreateAssetMenu(fileName = "New CrowdControl Immunity Effect", menuName = "Game/Effect/CrowdControlImmunity")]
  public class CrowdControlImmunity : Effect
	{

    [SerializeField]
    [Tooltip("Le nombre de seconde que l'immunité durera")]
	  public float duration;

	  private float endTime;

	  public override void ApplyOn(LivingEntity entity)
	  {
	    endTime = Time.time + duration;
	    entity.StartCoroutine(RepeatCleanse(entity.GetCrowdControl()));
	  }

	  private IEnumerator RepeatCleanse(CrowdControl crowdControl)
	  {
	    while (Time.time < endTime)
	    {
	      crowdControl.ReduceSnareCount();
	      crowdControl.ReduceStunCount();
        crowdControl.ResetSpeed(true);
        yield return null;
	    }
	  }
	}
}