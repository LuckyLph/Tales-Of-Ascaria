using UnityEngine;

namespace TalesOfAscaria
{
  public class WeaselDeadState : DeadState
  {
    private const float ExplosionOdds = 100f;

    public WeaselDeadState(MobController mob) : base(mob)
    {
      float random = Random.Range(1f, 100f);
      WeaselController weaselController = (WeaselController)MobController;
      if (random <= ExplosionOdds)
      {
        weaselController.MobDeathComplete(true);
      }
      weaselController.MobDeathComplete(false);
    }

    public override void Update()
    {
      base.Update();
    }
  }
}