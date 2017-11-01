using UnityEngine;
using System.Collections;

namespace TalesOfAscaria
{
  public class WeaselReturnToPatrolState : ReturnToPatrolState
  {
    public WeaselReturnToPatrolState(MobController mobController, Transform patrolPoint) : base(mobController, patrolPoint)
    {

    }

    public override void Update()
    {
      base.Update();
    }
  }
}