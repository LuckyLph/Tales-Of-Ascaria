using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class WeaselCore : MobCore
  {
    private const int weaselAttackAmount = 1;

    private StatsSnapshot attackStats;
    private Vector2 directionToFace;

    protected override void Awake()
    {
      base.Awake();
      timeStamps = new float[weaselAttackAmount];
      attacksCooldown = new float[weaselAttackAmount];
      cooldownNotified = new bool[weaselAttackAmount];

      attacksCooldown[0] = primaryAttackCooldown;
    }

    public override void UsePrimaryAttack(StatsSnapshot statsSnapshot, Vector2 directionToFace)
    {
      const int tabIndex = 0;

      if (Time.time >= attacksCooldown[tabIndex] + timeStamps[tabIndex])
      {
        timeStamps[tabIndex] = Time.time;
        cooldownNotified[tabIndex] = false;

        attackStats = statsSnapshot;
        this.directionToFace = directionToFace;
        attackAnimator.SetAnimationState(R.S.AnimatorParameter.IsAttacking, true);
      }
    }

    public void CreatePrimaryAttackHitbox()
    {
      GameObject primaryAttackClone = Instantiate(primaryAttackPrefab, transform.parent.transform.position, Quaternion.identity);
      primaryAttackClone.GetComponentInChildren<WeaselPrimaryAttackController>().SetWeaselPrimaryAttackControllerParameters(
      attackStats, directionToFace, primaryAttackClone);
    }
  }
}