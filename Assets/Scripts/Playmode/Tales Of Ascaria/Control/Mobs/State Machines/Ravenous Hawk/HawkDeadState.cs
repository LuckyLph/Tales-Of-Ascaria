using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class HawkDeadState : DeadState 
	{
    public HawkDeadState(MobController mobController) : base(mobController)
    {
      mobController.MobDeathComplete();
    }

    public override void Update()
    {
      base.Update();
    }
  }
}