using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public abstract class MobCore : GameScript
  {
    [Tooltip("The primary attack's prefab")]
    [SerializeField]
    protected GameObject primaryAttackPrefab;

    [Tooltip("The cooldown of the primary attack in seconds")]
    [SerializeField]
    protected float primaryAttackCooldown;

    protected LivingEntity livingEntity;
    protected StaticAnimator attackAnimator;

    protected float[] timeStamps;
    protected float[] attacksCooldown;
    protected bool[] cooldownNotified;

    private void InjectMobCore([GameObjectScope] LivingEntity livingEntity,
                               [GameObjectScope] StaticAnimator attackAnimator)
    {
      this.livingEntity = livingEntity;
      this.attackAnimator = attackAnimator;
    }

    protected virtual void Awake()
    {
      InjectDependencies("InjectMobCore");
    }

    public abstract void UsePrimaryAttack(StatsSnapshot statsSnapshot, Vector2 direction);
  }
}