using Harmony;

namespace TalesOfAscaria
{
  public class LevelUpOnLevelUp : GameScript
  {
    private ExperienceLevel experienceLevel;
    private LivingEntity livingEntity;

    private void InjectLevelUpOnLevelUp([GameObjectScope] ExperienceLevel experienceLevel,
                                        [GameObjectScope] LivingEntity livingEntity)
    {
      this.experienceLevel = experienceLevel;
      this.livingEntity = livingEntity;
    }

    private void Awake()
    {
      InjectDependencies("InjectLevelUpOnLevelUp");
    }

    private void OnEnable()
    {
      experienceLevel.OnLevelUp += OnLevelUp;
    }

    private void OnDisable()
    {
      experienceLevel.OnLevelUp -= OnLevelUp;
    }

    private void OnLevelUp(float maxHealthGained, float maxManaGained,
                           float strengthGained, float wisdomGained,
                           float constitutionGained, float spiritGained,
                           float dexterityGained, float healthRegenGained,
                           float manaRegenGained)
    {
      livingEntity.GetStats().IncreaseBaseStatsAccordingToGrowth(strengthGained, wisdomGained,
                                                                 constitutionGained, spiritGained,
                                                                 dexterityGained, healthRegenGained,
                                                                 manaRegenGained);
      livingEntity.GetHealth().ChangeMaxHealth(maxHealthGained);
      livingEntity.GetMana().IncreaseMaxMana(maxManaGained);
    }
  }
}