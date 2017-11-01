using System.Collections;
using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Raw Stats Adder", menuName = "Game/Effect/Raw Stats Adder")]
  public class RawStatsAdder : Effect, ICleansable
  {
    [Tooltip("The amound to add the target's strength. Can be negative.")]
    [SerializeField]
    private int strengthBonus = 0;

    [Tooltip("The amound to add the target's wisdom. Can be negative.")]
    [SerializeField]
    private int wisdomBonus = 0;

    [Tooltip("The amound to add the target's constitution. Can be negative.")]
    [SerializeField]
    private int constitutionBonus = 0;

    [Tooltip("The amound to add the target's spirit. Can be negative.")]
    [SerializeField]
    private int spiritBonus = 0;

    [Tooltip("The amound to add the target's agility. Can be negative.")]
    [SerializeField]
    private int agilityBonus = 0;

    [Tooltip("The amound to add the target's dexterity. Can be negative.")]
    [SerializeField]
    private int dexterityBonus = 0;

    [Tooltip("The amound to add the target's health regeneration. Can be negative.")]
    [SerializeField]
    private int healthRegenBonus = 0;

    [Tooltip("The amound to add the target's mana regeneration. Can be negative.")]
    [SerializeField]
    private int manaRegenBonus = 0;

    [Tooltip("Cet effet doit-il être enlevé en cas de cleanse?")]
    [SerializeField]
    private bool cleansable;

    [Tooltip("The duration of the effect in seconds")]
    [SerializeField]
    private float duration;

    public int StrengthBonus
    {
      get { return strengthBonus; }
      set { strengthBonus = value; }
    }

    public int WisdomBonus
    {
      get { return wisdomBonus; }
      set { wisdomBonus = value; }
    }

    public int ConstitutionBonus
    {
      get { return constitutionBonus; }
      set { constitutionBonus = value; }
    }

    public int SpiritBonus
    {
      get { return spiritBonus; }
      set { spiritBonus = value; }
    }

    public int AgilityBonus
    {
      get { return agilityBonus; }
      set { agilityBonus = value; }
    }

    public int DexterityBonus
    {
      get { return dexterityBonus; }
      set { dexterityBonus = value; }
    }

    public int HealthRegenBonus
    {
      get { return healthRegenBonus; }
      set { healthRegenBonus = value; }
    }

    public int ManaRegenBonus
    {
      get { return manaRegenBonus; }
      set { manaRegenBonus = value; }
    }

    public float Duration
    {
      get { return duration; }
      set { duration = value; }
    }

    private float endTime;


    public override void ApplyOn(LivingEntity entity)
    {
      endTime = Time.time + Duration;
      entity.OnCleanse += Cleanse;
      entity.StartCoroutine(ApplyBonuses(entity));
    }


    private IEnumerator ApplyBonuses(LivingEntity entity)
    {
      AbsoluteStatBonus bonus = new AbsoluteStatBonus(
        StrengthBonus,
        WisdomBonus,
        ConstitutionBonus,
        SpiritBonus,
        AgilityBonus,
        DexterityBonus,
        HealthRegenBonus,
        ManaRegenBonus);
      entity.GetStats().AddAbsoluteBonus(bonus);
      yield return new WaitUntil(() => Time.time >= endTime);
      entity.GetStats().RemoveAbsoluteBonus(bonus);
      yield return null;
    }

    public void Cleanse(LivingEntity entity)
    {
      if (cleansable)
      {
        endTime = Time.time;
      }
    }
  }
}