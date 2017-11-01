using System.Collections.Generic;
using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Représente la gestion des statistiques d'une entité vivante.
  /// </summary>
  public class Stats : GameScript
  {
    /// <summary>
    /// Capture des statistiques avant les bonus.
    /// </summary>
    private StatsSnapshot baseStats;

    /// <summary>
    /// Liste des bonus de statistiques en valeurs absolues.
    /// </summary>
    private List<AbsoluteStatBonus> absoluteStatBonuses;

    /// <summary>
    /// Liste des bonus de statistiques en multiplicateurs.
    /// </summary>
    private List<StatMultiplierBonus> statMultiplierBonuses;

    public const int MaximumDexterity = 60;
    public const int MinimumStat = 0;

    [Tooltip("Puissance des attaques physique")]
    [SerializeField]
    private float baseStrength = MinimumStat;

    [Tooltip("Puissance des attaques magique")]
    [SerializeField]
    private float baseWisdom = MinimumStat;

    [Tooltip("Défense physique")]
    [SerializeField]
    private float baseConstitution = MinimumStat;

    [Tooltip("Défense magique")]
    [SerializeField]
    private float baseSpirit = MinimumStat;

    [Tooltip("Vitesse")]
    [SerializeField]
    private float baseAgility = MinimumStat;

    [Tooltip("Taux de réduction des cooldowns (pourcentage)")]
    [Range(0, 60)]
    [SerializeField]
    private float baseDexterity = MinimumStat;

    [Tooltip("Régénération de points de vie")]
    [SerializeField]
    private float baseHealthRegen = MinimumStat;

    [Tooltip("Régénération de points de mana")]
    [SerializeField]
    private float baseManaRegen = MinimumStat;

    public float Strength
    {
      get
      {
        float finalStrength = baseStrength;
        for (int i = 0; i < absoluteStatBonuses.Count; i++)
        {
          finalStrength += absoluteStatBonuses[i].BonusStrength;
        }
        for (int i = 0; i < statMultiplierBonuses.Count; i++)
        {
          finalStrength *= statMultiplierBonuses[i].StrengthModifier;
        }
        ReadjustStatsToMinimum(ref finalStrength);
        return finalStrength;
      }
    }

    public float Wisdom
    {
      get
      {
        float finalWisdom = baseWisdom;
        for (int i = 0; i < absoluteStatBonuses.Count; i++)
        {
          finalWisdom += absoluteStatBonuses[i].BonusWisdom;
        }
        for (int i = 0; i < statMultiplierBonuses.Count; i++)
        {
          finalWisdom *= statMultiplierBonuses[i].WisdomModifier;
        }
        ReadjustStatsToMinimum(ref finalWisdom);
        return finalWisdom;
      }
    }

    public float Constitution
    {
      get
      {
        float finalConstitution = baseConstitution;
        for (int i = 0; i < absoluteStatBonuses.Count; i++)
        {
          finalConstitution += absoluteStatBonuses[i].BonusConstitution;
        }
        for (int i = 0; i < statMultiplierBonuses.Count; i++)
        {
          finalConstitution *= statMultiplierBonuses[i].ConstitutionModifier;
        }
        ReadjustStatsToMinimum(ref finalConstitution);
        return finalConstitution;
      }
    }

    public float Spirit
    {
      get
      {
        float finalSpirit = baseSpirit;
        for (int i = 0; i < absoluteStatBonuses.Count; i++)
        {
          finalSpirit += absoluteStatBonuses[i].BonusSpirit;
        }
        for (int i = 0; i < statMultiplierBonuses.Count; i++)
        {
          finalSpirit *= statMultiplierBonuses[i].SpiritModifier;
        }
        ReadjustStatsToMinimum(ref finalSpirit);
        return finalSpirit;
      }
    }

    public float Agility
    {
      get
      {
        float finalAgility = baseAgility;
        for (int i = 0; i < absoluteStatBonuses.Count; i++)
        {
          finalAgility += absoluteStatBonuses[i].BonusAgility;
        }
        for (int i = 0; i < statMultiplierBonuses.Count; i++)
        {
          finalAgility *= statMultiplierBonuses[i].AgilityModifier;
        }
        ReadjustStatsToMinimum(ref finalAgility);
        return finalAgility;
      }
    }

    public float Dexterity
    {
      get
      {
        float finalDexterity = baseDexterity;
        for (int i = 0; i < absoluteStatBonuses.Count; i++)
        {
          finalDexterity += absoluteStatBonuses[i].BonusDexterity;
        }
        for (int i = 0; i < statMultiplierBonuses.Count; i++)
        {
          finalDexterity *= statMultiplierBonuses[i].DexterityModifier;
        }
        if (finalDexterity > MaximumDexterity)
        {
          return MaximumDexterity;
        }
        ReadjustStatsToMinimum(ref finalDexterity);
        return finalDexterity;
      }
    }

    public float HealthRegen
    {
      get
      {
        float finalHealthRegen = baseHealthRegen;
        for (int i = 0; i < absoluteStatBonuses.Count; i++)
        {
          finalHealthRegen += absoluteStatBonuses[i].BonusHealthRegen;
        }
        for (int i = 0; i < statMultiplierBonuses.Count; i++)
        {
          finalHealthRegen *= statMultiplierBonuses[i].HealthRegenModifier;
        }
        return finalHealthRegen;
      }
    }

    public float ManaRegen
    {
      get
      {
        float finalManaRegen = baseManaRegen;
        for (int i = 0; i < absoluteStatBonuses.Count; i++)
        {
          finalManaRegen += absoluteStatBonuses[i].BonusManaRegen;
        }
        for (int i = 0; i < statMultiplierBonuses.Count; i++)
        {
          finalManaRegen *= statMultiplierBonuses[i].ManaRegenModifier;
        }
        return finalManaRegen;
      }
    }

    public void Awake()
    {
      baseStats = new StatsSnapshot(baseStrength, baseWisdom, baseConstitution, baseSpirit,
                                    baseAgility, baseDexterity, baseHealthRegen, baseManaRegen);
      statMultiplierBonuses = new List<StatMultiplierBonus>();
      absoluteStatBonuses = new List<AbsoluteStatBonus>();
    }

    /// <summary>
    /// Ajoute un bonus de valeur absolu à la liste de bonus absolues.
    /// </summary>
    /// <param name="bonus">bonus valeur absolu à ajouter</param>
    public void AddAbsoluteBonus(AbsoluteStatBonus bonus)
    {
      absoluteStatBonuses.Add(bonus);
    }

    /// <summary>
    /// Retire un bonus de valeur absolu à la liste de bonus absolues.
    /// </summary>
    /// <param name="bonus">bonus valeur absolu à ajouter</param>
    public void RemoveAbsoluteBonus(AbsoluteStatBonus bonus)
    {
      absoluteStatBonuses.Remove(bonus);
    }

    /// <summary>
    /// Ajoute un bonus de multiplicateur à la liste de bonus multiplicateurs
    /// </summary>
    /// <param name="bonus">bonus à multiplier</param>
    public void AddMultiplier(StatMultiplierBonus bonus)
    {
      statMultiplierBonuses.Add(bonus);
    }

    /// <summary>
    /// Retire un bonus de multiplicateur à la liste de bonus multiplicateurs
    /// </summary>
    /// <param name="bonus">bonus à multiplier</param>
    public void RemoveMultiplier(StatMultiplierBonus bonus)
    {
      statMultiplierBonuses.Remove(bonus);
    }

    /// <summary>
    /// Obtient une capture des statistiques après avoir appliqué les bonus absolu et les bonus multipliés.
    /// </summary>
    /// <returns>Snapshot après que les bonus soient appliqués</returns>
    public StatsSnapshot GetStatsSnapshot()
    {
      StatsSnapshot finalStatsSnapshot = new StatsSnapshot(baseStats);
      for (int i = 0; i < absoluteStatBonuses.Count; i++)
      {
        finalStatsSnapshot += absoluteStatBonuses[i];
      }
      for (int i = 0; i < statMultiplierBonuses.Count; i++)
      {
        finalStatsSnapshot *= statMultiplierBonuses[i];
      }
      ReduceStatsSnapshotToCappedStats(finalStatsSnapshot);
      ReadjustStatSnapshotToMinimum(finalStatsSnapshot);
      return finalStatsSnapshot;
    }

    public void IncreaseBaseStatsAccordingToGrowth(float strengthGrowth,
                                                   float wisdomGrowth,
                                                   float constitutionGrowth,
                                                   float spiritGrowth,
                                                   float dexterityGrowth,
                                                   float healthRegenGrowth,
                                                   float manaRegenGrowth)
    {
      baseStrength += strengthGrowth;
      baseWisdom += wisdomGrowth;
      baseConstitution += constitutionGrowth;
      baseSpirit += spiritGrowth;
      baseDexterity += dexterityGrowth;
      baseHealthRegen += healthRegenGrowth;
      baseManaRegen += manaRegenGrowth;

      baseStats = new StatsSnapshot(baseStrength, baseWisdom, baseConstitution, baseSpirit,
                                    baseAgility, baseDexterity, baseHealthRegen, baseManaRegen);
    }

    private void ReduceStatsSnapshotToCappedStats(StatsSnapshot statsSnapshot)
    {
      if (statsSnapshot.Dexterity > MaximumDexterity)
      {
        statsSnapshot.Dexterity = MaximumDexterity;
      }
    }

    private void ReadjustStatsToMinimum(ref float stat)
    {
      if (stat < MinimumStat)
      {
        stat = MinimumStat;
      }
    }

    private void ReadjustStatSnapshotToMinimum(StatsSnapshot statsSnapshot)
    {
      if (statsSnapshot.Strength < MinimumStat)
      {
        statsSnapshot.Strength = MinimumStat;
      }
      if (statsSnapshot.Wisdom < MinimumStat)
      {
        statsSnapshot.Wisdom = MinimumStat;
      }
      if (statsSnapshot.Constitution < MinimumStat)
      {
        statsSnapshot.Constitution = MinimumStat;
      }
      if (statsSnapshot.Spirit < MinimumStat)
      {
        statsSnapshot.Spirit = MinimumStat;
      }
      if (statsSnapshot.Agility < MinimumStat)
      {
        statsSnapshot.Agility = MinimumStat;
      }
      if (statsSnapshot.Dexterity < MinimumStat)
      {
        statsSnapshot.Dexterity = MinimumStat;
      }
    }
  }
}