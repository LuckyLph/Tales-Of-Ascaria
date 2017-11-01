using UnityEngine;

namespace TalesOfAscaria 
{
  /// <summary>
  /// Représente un effet de guérison instantanée
  /// </summary>
  [CreateAssetMenu(fileName = "New HealEffect", menuName = "Game/Effect/InstantHeal")]
  public class InstantHeal : Effect
	{
    [Tooltip("Le nombre de vie à ajouter")]
	  [SerializeField] private int healPower;

	  public void SetHeal(int healAmount)
	  {
	    healPower = healAmount;
	  }

	  public override void ApplyOn(LivingEntity entity)
	  {
	    entity.TakeUnreductibleDamage(-1 * healPower);
      Debug.Log("boy I just healed by: " + healPower);
	  }
	}
}