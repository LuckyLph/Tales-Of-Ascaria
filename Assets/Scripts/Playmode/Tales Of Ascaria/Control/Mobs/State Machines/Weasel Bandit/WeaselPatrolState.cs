using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Ouais je sais ce PatrolState en particulier est pas très différent de son parent...
  /// </summary>
  public class WeaselPatrolState : PatrolState
  {
    
    public WeaselPatrolState(MobController mobController, Transform patrolPoint, float patrolRange) : base(mobController, patrolPoint, patrolRange)
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