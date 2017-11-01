using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class HawkPatrolState : PatrolState 
	{
    public HawkPatrolState(MobController mobController, Transform patrolPoint, float patrolRange) : base(mobController, patrolPoint, patrolRange)
    {

    }

    public override void Update()
    {
      base.Update();
    }

    protected override void Move(bool isPatrolling)
    {
      base.Move(isPatrolling);
    }
  }
}