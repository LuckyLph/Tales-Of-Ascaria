using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class HawkAttackingState : AttackingState 
	{
    public HawkAttackingState(MobController mobController, Transform patrolPoint, float attackStateRange, float primaryAttackRange) : base(mobController, patrolPoint, attackStateRange, primaryAttackRange)
    {
    }

    public override void Update()
    {
      base.Update();
    }

    protected override void Attack()
    {
      base.Attack();
    }

    protected override void Move(bool isPatrolling)
    {
      base.Move(isPatrolling);
    }
  }
}