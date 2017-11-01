using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class HawkController : MobController 
	{

    protected override void Awake() 
		{
      base.Awake();
		}

    public override void Attack(MobAttackIndex attacksIndex, Vector2 directionToFace)
    {
      if (livingEntity.GetCrowdControl().SnareCounter <= 0 && livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        directionAnimator.SetDirection(directionToFace);
        switch (attacksIndex)
        {
          case MobAttackIndex.PrimaryAttack:
            mobCore.UsePrimaryAttack(stats.GetStatsSnapshot(), directionToFace);
            break;
        }
      }
    }

    public override void MobDeathComplete()
    {
      base.MobDeathComplete();
    }
  }
}