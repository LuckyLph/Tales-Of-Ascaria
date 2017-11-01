using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class HawkCore : MobCore 
	{
    private const int hawkAttackAmount = 1;

    protected override void Awake() 
		{
      base.Awake();
      timeStamps = new float[hawkAttackAmount];
      attacksCooldown = new float[hawkAttackAmount];
      cooldownNotified = new bool[hawkAttackAmount];

      attacksCooldown[0] = primaryAttackCooldown;
    }

    public override void UsePrimaryAttack(StatsSnapshot statsSnapshot, Vector2 direction)
    {
      const int tabIndex = 0;

      if (Time.time >= attacksCooldown[tabIndex] + timeStamps[tabIndex])
      {
        timeStamps[tabIndex] = Time.time;
        cooldownNotified[tabIndex] = false;
        attackAnimator.SetAnimationState(R.S.AnimatorParameter.IsAttacking, true);

        //GameObject primaryAttackClone = Instantiate(primaryAttackPrefab, transform.parent.transform.position, Quaternion.identity);
        //primaryAttackClone.GetComponentInChildren<WeaselPrimaryAttackController>().SetWeaselPrimaryAttackControllerParameters(
        //statsSnapshot, direction, primaryAttackClone);
      }
    }
  }
}