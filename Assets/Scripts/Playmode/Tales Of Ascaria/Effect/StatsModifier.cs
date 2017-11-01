using System;
using System.Collections;
using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Stats Modifier Effect", menuName = "Game/Effect/Stats Modifier")]
  public class StatsModifier : Effect, ICleansable
  {
    [Tooltip("The percentage to change the target's strength. From 0 to 1 decreases the stat. Over 1 increases the stat.")]
    [SerializeField] private float strengthModifier = 1;

    [Tooltip("The percentage to change the target's wisdom. From 0 to 1 decreases the stat. Over 1 increases the stat.")]
    [SerializeField] private float wisdomModifier = 1;

    [Tooltip("The percentage to change the target's constitution. From 0 to 1 decreases the stat. Over 1 increases the stat.")]
    [SerializeField] private float constitutionModifier = 1;

    [Tooltip("The percentage to change the target's spirit. From 0 to 1 decreases the stat. Over 1 increases the stat.")]
    [SerializeField] private float spiritModifier = 1;

    [Tooltip("The percentage to change the target's agility. From 0 to 1 decreases the stat. Over 1 increases the stat.")]
    [SerializeField] private float agilityModifier = 1;

    [Tooltip("The percentage to change the target's dexterity. From 0 to 1 decreases the stat. Over 1 increases the stat.")]
    [SerializeField] private float dexterityModifier = 1;

    [Tooltip("The percentage to change the target's health regen. From 0 to 1 decreases the stat. Over 1 increases the stat.")]
    [SerializeField] private float healthRegenModifier = 1;

    [Tooltip("The percentage to change the target's mana regen. From 0 to 1 decreases the stat. Over 1 increases the stat.")]
    [SerializeField] private float manaRegenModifier = 1;

    [Tooltip("The percentage to change the target's general damage output modifier. From 0 to 1 decreases the stat. Over 1 increases the stat.")]
    [SerializeField] private float damageOutputModifier = 1;

    [Tooltip("The percentage to change the target's general damage intake modifier. From 0 to 1 decreases the stat. Over 1 increases the stat.")]
    [SerializeField] private float damageIntakeModifier = 1;

    [Tooltip("The duration of the effect in seconds")]
    [SerializeField] private float duration;

    [Tooltip("If the entity should become 'invisible' during the effect. Removes monster aggro and prevents it.")]
    [SerializeField] private bool isInvisible;

    [Tooltip("If the effect should be removeable.")]
    [SerializeField]
    private bool isCleansable;

    public float StrengthModifier
    {
      get { return strengthModifier; }
      set { strengthModifier = value; }
    }

    public float WisdomModifier
    {
      get { return wisdomModifier; }
      set { wisdomModifier = value; }
    }

    public float ConstitutionModifier
    {
      get { return constitutionModifier; }
      set { constitutionModifier = value; }
    }

    public float SpiritModifier
    {
      get { return spiritModifier; }
      set { spiritModifier = value; }
    }

    public float AgilityModifier
    {
      get { return agilityModifier; }
      set { agilityModifier = value; }
    }

    public float DexterityModifier
    {
      get { return dexterityModifier; }
      set { dexterityModifier = value; }
    }

    public float HealthRegenModifier
    {
      get { return healthRegenModifier; }
      set { healthRegenModifier = value; }
    }

    public float ManaRegenModifier
    {
      get { return manaRegenModifier; }
      set { manaRegenModifier = value; }
    }

    public float Duration
    {
      get { return duration; }
      set { duration = value; }
    }

    public bool IsInvisible
    {
      get { return isInvisible; }
      set { isInvisible = value; }
    }

    public float DamageIntakeModifier
    {
      get { return damageIntakeModifier; }
      set { damageIntakeModifier = value; }
    }

    public float DamageOutputModifier
    {
      get { return damageOutputModifier; }
      set { damageOutputModifier = value; }
    }

    private float endTime;

    public override void ApplyOn(LivingEntity entity)
    {
      endTime = Time.time + Duration;
      entity.OnCleanse += Cleanse;
      entity.StartCoroutine(ApplyDefensiveModifier(entity));
    }


    private IEnumerator ApplyDefensiveModifier(LivingEntity entity)
    {
      endTime = Time.time + duration;
      StatMultiplierBonus modifier = new StatMultiplierBonus(
                                                             StrengthModifier,
                                                             WisdomModifier,
                                                             ConstitutionModifier,
                                                             SpiritModifier,
                                                             AgilityModifier,
                                                             DexterityModifier,
                                                             HealthRegenModifier,
                                                             ManaRegenModifier,
                                                             damageOutputModifier,
                                                             damageIntakeModifier);
      entity.GetStats().AddMultiplier(modifier);
      entity.IsInvisible = isInvisible;
      while (endTime > Time.time)
      {
        yield return new WaitForEndOfFrame();
      }
      entity.GetStats().RemoveMultiplier(modifier);
      entity.IsInvisible = false;
      yield return null;
    }

    public void Cleanse(LivingEntity entity)
    {
      if (isCleansable)
      {
        EndEffect();
      }
    }

    public void EndEffect()
    {
      endTime = Time.time;
    }
  }
}
