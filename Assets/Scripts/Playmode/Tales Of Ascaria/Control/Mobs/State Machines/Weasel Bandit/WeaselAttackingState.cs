using UnityEngine;

namespace TalesOfAscaria
{
  public class WeaselAttackingState : AttackingState
  {
    public WeaselAttackingState(MobController mob, Transform patrolPoint, float attackStateRange, float primaryAttackRange) :
    base(mob, patrolPoint, attackStateRange, primaryAttackRange)
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