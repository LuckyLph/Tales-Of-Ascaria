using UnityEngine;
using System.Collections;

namespace TalesOfAscaria
{
  public class ReturnToPatrolState : GameState
  {
    protected Transform patrolPoint;

    public ReturnToPatrolState(MobController mobController, Transform patrolPoint) : base(mobController)
    {
      this.patrolPoint = patrolPoint;
      destination = patrolPoint.position;
    }

    public override void Update()
    {
      base.Update();

      if (Vector2.Distance(currentPosition, patrolPoint.transform.position) < proximityRadius)
      {
        InvokeOnNewState(GameStates.PatrolState);
      }
      else
      {
        Move(false);
      }
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