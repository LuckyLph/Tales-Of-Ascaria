using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class HawkReturnToPatrolState : ReturnToPatrolState 
	{
    public HawkReturnToPatrolState(MobController mobController, Transform patrolPoint) : base(mobController, patrolPoint)
    {

    }

    public override void Update()
    {
      base.Update();
    }
  }
}