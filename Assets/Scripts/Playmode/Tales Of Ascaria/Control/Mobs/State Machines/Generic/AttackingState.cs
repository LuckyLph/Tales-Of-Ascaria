using UnityEngine;

namespace TalesOfAscaria
{
  public abstract class AttackingState : GameState
  {
    private const float directionVectorMargin = 0.7f;

    protected Transform patrolPoint;
    protected float attackStateRange;
    protected float primaryAttackRange;

    public AttackingState(MobController mobController, Transform patrolPoint,
                          float attackStateRange, float primaryAttackRange) : base(mobController)
    {
      this.primaryAttackRange = primaryAttackRange;
      this.attackStateRange = attackStateRange;
      this.patrolPoint = patrolPoint;
    }

    public override void Update()
    {
      base.Update();

      if (MobController.PlayersInRange.Count <= 0 || Vector2.Distance(currentPosition, patrolPoint.position) > attackStateRange)
      {
        InvokeOnNewState(GameStates.ReturningToPatrolState);
        return;
      }

      MobController.Target = MobController.Target ?? (MobController.Target = MobController.PlayersInRange[0]);
      destination = MobController.Target.transform.position;

      if (Vector2.Distance(currentPosition, MobController.Target.transform.position) > primaryAttackRange)
      {
        Move(false);
      }
      else
      {
        Attack();
      }
    }
    
    protected virtual void Attack()
    {
      Vector2 directionToFace = new Vector2();
      directionToFace = (Vector2)MobController.Target.transform.position - currentPosition;
      directionToFace = directionToFace.normalized;
      if (directionToFace.x >= directionVectorMargin && (directionToFace.y <= directionVectorMargin && directionToFace.y >= -directionVectorMargin))
      {
        directionToFace = new Vector2(1, 0);
      }
      else if (directionToFace.x <= -directionVectorMargin && (directionToFace.y <= directionVectorMargin && directionToFace.y >= -directionVectorMargin))
      {
        directionToFace = new Vector2(-1, 0);
      }
      else if (directionToFace.y > -directionVectorMargin && (directionToFace.x < directionVectorMargin && directionToFace.x > -directionVectorMargin))
      {
        directionToFace = new Vector2(0, 1);
      }
      else if (directionToFace.y < directionVectorMargin && (directionToFace.x < directionVectorMargin && directionToFace.x > -directionVectorMargin))
      {
        directionToFace = new Vector2(0, -1);
      }

      MobController.Attack(MobAttackIndex.PrimaryAttack, directionToFace);
      
    }

    protected override void Move(bool isPatrolling)
    {
      base.Move(isPatrolling);
    }

    protected override void CheckIfXSmallestDistance()
    {
      base.CheckIfXSmallestDistance();
    }
  }
}