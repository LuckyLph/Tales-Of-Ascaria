using Harmony;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TalesOfAscaria
{
  public delegate void CleanseHandler(LivingEntity entity);

  public class LivingEntity : GameScript
  {   
    public event CleanseHandler OnCleanse;

    public bool IsInvisible
    {
      get { return isInvisible; }
      set { isInvisible = value; }
    }

    private Health health;
    private Mana mana;
    private Stats stats;
    private ExperienceLevel experienceLevel;
    private bool isInvisible;
    private CrowdControl crowdControl;
    private AudioSource audioSource;

    private void InjectLivingEntity([GameObjectScope] Health health,
                                    [GameObjectScope] Stats stats,
                                    [GameObjectScope] ExperienceLevel experienceLevel,
                                    [GameObjectScope] CrowdControl crowdControl,
                                    [GameObjectScope] Mana mana)
    {
      this.health = health;
      this.mana = mana;
      this.stats = stats;
      this.experienceLevel = experienceLevel;
      this.crowdControl = crowdControl;
    }

    private void Awake()
    {
      InjectDependencies("InjectLivingEntity");
    }

    /// <summary>
    /// Inflige des dégats à l'entité en appliquant les réductions.
    /// </summary>
    /// <param name="damage">Le nombre de points de vie à réduire</param>
    /// <param name="damageType">Le type de dégats</param>
    public void TakeReductibleDamage(float damage, DamageType damageType)
    {
      StatsSnapshot snapshot = stats.GetStatsSnapshot();
      damage *= snapshot.FinalDamageReceivedModifier;
      if (damageType == DamageType.Magical)
      {
        health.TakeDamage((int)(100f / (100f + snapshot.Spirit) * damage));
      }
      else
      {
        health.TakeDamage((int)(100f / (100f + snapshot.Constitution) * damage));
      }
      Debug.Log("I've taken damage! My health is now : " + health.HealthPoints);
    }

    /// <summary>
    /// Inflige des dégats sans appliquer les réductions.
    /// </summary>
    /// <param name="damage">Le nombe de points de vie à réduire</param>
    public void TakeUnreductibleDamage(float damage)
    {
      health.TakeDamage(damage);
    }

    public void Heal(float healPoints)
    {
      if (health.HealthPoints + healPoints > health.MaximumHealthPoints)
      {
        health.RegainHealth();
      }
      else
      {
        health.Heal(healPoints);
      }
    }

    public void ReduceMana(int manaCost)
    {
      mana.UseMana(manaCost);
    }


    public void Cleanse()
    {
      if (OnCleanse != null) OnCleanse(this);
    }


    public bool CanCastSpell(int manaCost)
    {
      return mana.HasEnoughMana(manaCost) && crowdControl.StunCounter <= 0;
    }

    public int GetLevel()
    {
      return (experienceLevel == null) ? experienceLevel.GetLevel() : 0;
    }

    public void AddExperience(int gainedExperience)
    {
      experienceLevel.AddExperience(gainedExperience);
    }

    public void LoseExperience()
    {
      experienceLevel.ApplyPenalty();
    }

    public Health GetHealth()
    {
      return health;
    }

    public Mana GetMana()
    {
      return mana;
    }

    public CrowdControl GetCrowdControl()
    {
      return crowdControl;
    }

    public Stats GetStats()
    {
      return stats;
    }
  }
}
