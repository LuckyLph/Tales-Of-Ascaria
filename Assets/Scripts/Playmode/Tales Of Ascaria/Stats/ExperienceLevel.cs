using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void OnLevelUpHandler(float maxHealthGained,
                                        float maxManaGained,
                                        float strengthGained,
                                        float wisdomGained,
                                        float constitutionGained,
                                        float spiritGained,
                                        float dexterityGained,
                                        float healthRegenGained,
                                        float manaRegenGained);

  public class ExperienceLevel : GameScript
  {
    public event OnLevelUpHandler OnLevelUp;

    /// <summary>
    /// Nombre total d'expérience avant les levelups.
    /// </summary>
    public int TotalExperience { get; set; }

    /// <summary>
    /// Expérience de l'entité.
    /// </summary>
    public int Experience { get; set; }

    [Tooltip("Niveau actuel de l'entité (ne pas monter le niveau du joueur)")]
    [SerializeField]
    private int level;

    [Tooltip("Taux d'augmentation des points de vie maximaux")]
    [SerializeField]
    private float healthPointsGrowth;

    [Tooltip("Taux d'augmentation des points de vie maximaux")]
    [SerializeField]
    private float manaPointsGrowth;

    [Tooltip("Taux d'augmentation de strength")]
    [SerializeField]
    private float strengthGrowth;

    [Tooltip("Taux d'augmentation de wisdom")]
    [SerializeField]
    private float wisdomGrowth;

    [Tooltip("Taux d'augmentation de constitution")]
    [SerializeField]
    private float constitutionGrowth;

    [Tooltip("Taux d'augmentation de spirit")]
    [SerializeField]
    private float spiritGrowth;

    [Tooltip("Taux d'augmentation de dexterity")]
    [Range(0, 0.6f)]
    [SerializeField]
    private float dexterityGrowth;

    [Tooltip("Taux d'augmentation de health regen")]
    [SerializeField]
    private float healthRegenGrowth;

    [Tooltip("Taux d'augmentation de mana regen")]
    [SerializeField]
    private float manaRegenGrowth;

    public int GetLevel()
    {
      return level;
    }

    /// <summary>
    /// Set le level de l'entité, utiliser seulement lors 
    /// </summary>
    /// <param name="newLevel">Le nouveau niveau</param>
    public void SetLevel(int newLevel)
    {
      level = newLevel;
      if (OnLevelUp != null)
        OnLevelUp(newLevel * healthPointsGrowth, newLevel * manaPointsGrowth,
                  newLevel * strengthGrowth, newLevel * wisdomGrowth,
                  newLevel * constitutionGrowth, newLevel * spiritGrowth,
                  newLevel * dexterityGrowth, newLevel * healthRegenGrowth, newLevel * manaPointsGrowth);
    }

    /// <summary>
    /// Ajoute de l'expérience au joueur et vérifie pour un levelup.
    /// </summary>
    /// <param name="gainedExperience"></param>
    public void AddExperience(int gainedExperience)
    {
      TotalExperience += gainedExperience;
      Experience += gainedExperience;
      CheckForLevelUp();
    }

    /// <summary>
    /// Vérifie récursivement si le joueur a assez d'expérience pour monter de niveau.
    /// Si c'est le cas, la méthode se rappèle après avoir soustrait l'expérience.
    /// </summary>
    private void CheckForLevelUp()
    {
      int experienceToLevelup = (int)(531 * Mathf.Pow(1.03f, GetLevel()) - 200);
      if (Experience >= experienceToLevelup)
      {
        Experience -= experienceToLevelup;
        level += 1;
        if (OnLevelUp != null)
          OnLevelUp(healthPointsGrowth, manaPointsGrowth,
                    strengthGrowth, wisdomGrowth,
                    constitutionGrowth, spiritGrowth,
                    dexterityGrowth, healthRegenGrowth, manaPointsGrowth);
        CheckForLevelUp();
      }
    }

    public void ApplyPenalty()
    {
      TotalExperience -= Experience;
      Experience = 0;
    }
  }
}