using System.Collections;
using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Damage Effect", menuName = "Game/Effect/Damage")]
  public class DamageOverTime : Effect, ICleansable
  {
    [Tooltip("Duration of the effect in seconds")]
    [SerializeField] private int durationInSeconds;

    [Tooltip("Amount of health points lost per tick from target.")]
    [SerializeField] private int baseDamagePerTick;

    [Tooltip("Number of ticks per second.")]
    [SerializeField] private float ticksPerSecond;

    [Tooltip("Type of damage. True damage will ignore constitution and spirit.")]
    [SerializeField] private DamageType damageType;

    [Tooltip("Type of Damage over time. Only changes the visual effect.")]
    [SerializeField] private DamageOverTimeType damageOverTimeType;

    private float damageEndTime;

    public float BonusDamagePerTick { get; set; }

    public override void ApplyOn(LivingEntity entity)
    {
      entity.OnCleanse += Cleanse;
      entity.StartCoroutine(ApplyDamageOn(entity));
    }

    private IEnumerator ApplyDamageOn(LivingEntity entity)
    {
      float damagePerTick = baseDamagePerTick + BonusDamagePerTick;
      damageEndTime = Time.time + durationInSeconds;
      while (Time.time < damageEndTime)
      {
        yield return new WaitForSeconds(1 / ticksPerSecond);
        if (damageType == DamageType.True)
        {
          entity.TakeUnreductibleDamage(damagePerTick);
        }
        else
        {
          entity.TakeReductibleDamage(damagePerTick,damageType);
        }
      }
    }

    public void Cleanse(LivingEntity entity)
    {
      damageEndTime = Time.time;
    }
  }
}