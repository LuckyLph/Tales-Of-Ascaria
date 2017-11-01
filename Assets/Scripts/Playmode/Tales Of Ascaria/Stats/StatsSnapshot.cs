namespace TalesOfAscaria
{
  /// <summary>
  /// Représente une capture de statistiques.
  /// </summary>
  public class StatsSnapshot
  {
    public float Strength { get; set; }
    public float Wisdom { get; set; }
    public float Constitution { get; set; }
    public float Spirit { get; set; }
    public float Agility { get; set; }
    public float Dexterity { get; set; }
    public float HealthRegen { get; set; }
    public float ManaRegen { get; set; }
    public float FinalDamageDealtModifier { get; set; }
    public float FinalDamageReceivedModifier { get; set; }

    public StatsSnapshot(float strength,
                         float wisdom,
                         float constitution,
                         float spirit,
                         float agility,
                         float dexterity,
                         float healthRegen,
                         float manaRegen)
    {
      Strength = strength;
      Wisdom = wisdom;
      Constitution = constitution;
      Spirit = spirit;
      Agility = agility;
      Dexterity = dexterity;
      HealthRegen = healthRegen;
      ManaRegen = manaRegen;
      FinalDamageDealtModifier = 1;
      FinalDamageReceivedModifier = 1;
    }

    public StatsSnapshot(StatsSnapshot stats)
    {
      Strength = stats.Strength;
      Wisdom = stats.Wisdom;
      Constitution = stats.Constitution;
      Spirit = stats.Spirit;
      Agility = stats.Agility;
      Dexterity = stats.Dexterity;
      HealthRegen = stats.HealthRegen;
      ManaRegen = stats.ManaRegen;
      FinalDamageDealtModifier = 1;
      FinalDamageReceivedModifier = 1;
    }

    public static StatsSnapshot operator +(StatsSnapshot stats, AbsoluteStatBonus bonus)
    {
      stats.Strength += bonus.BonusStrength;
      stats.Wisdom += bonus.BonusWisdom;
      stats.Constitution += bonus.BonusConstitution;
      stats.Spirit += bonus.BonusSpirit;
      stats.Agility += bonus.BonusAgility;
      stats.Dexterity += bonus.BonusDexterity;
      stats.HealthRegen += bonus.BonusHealthRegen;
      stats.ManaRegen += bonus.BonusManaRegen;
      return stats;
    }

    public static StatsSnapshot operator +(StatsSnapshot stats, StatsSnapshot addedStats)
    {
      stats.Strength += addedStats.Strength;
      stats.Wisdom += addedStats.Wisdom;
      stats.Constitution += addedStats.Constitution;
      stats.Spirit += addedStats.Spirit;
      stats.Dexterity += addedStats.Dexterity;
      stats.HealthRegen += addedStats.HealthRegen;
      stats.ManaRegen += addedStats.ManaRegen;
      return stats;
    }

    public static StatsSnapshot operator *(StatsSnapshot stats, StatMultiplierBonus bonus)
    {
      stats.Strength *= bonus.StrengthModifier;
      stats.Wisdom *= bonus.WisdomModifier;
      stats.Constitution *= bonus.ConstitutionModifier;
      stats.Spirit *= bonus.SpiritModifier;
      stats.Agility *= bonus.AgilityModifier;
      stats.Dexterity *= bonus.DexterityModifier;
      stats.HealthRegen *= bonus.HealthRegenModifier;
      stats.ManaRegen *= bonus.ManaRegenModifier;
      stats.FinalDamageDealtModifier *= bonus.FinalDamageDealingModifier;
      stats.FinalDamageReceivedModifier *= bonus.FinalDamageRecievedModifier;
      return stats;
    }
  }
}