using UnityEngine;

namespace TalesOfAscaria 
{
  [CreateAssetMenu(fileName = "New Cleanse Effect", menuName = "Game/Effect/Cleanse")]
	public class Cleanse : Effect 
	{
	  public override void ApplyOn(LivingEntity entity)
	  {
	    entity.Cleanse();
	  }
	}
}