using Harmony;

namespace TalesOfAscaria
{
  public class LoseExperienceOnDeath : GameScript
  {
    private LivingEntity livingEntity;
    private Health health;

    private void InjectExperienceOnDeath([GameObjectScope] LivingEntity livingEntity,
                                        [GameObjectScope] Health health)
    {
      this.livingEntity = livingEntity;
      this.health = health;
    }

    private void Awake()
    {
      InjectDependencies("InjectExperienceOnDeath");
    }

    private void OnEnable()
    {
      health.OnDeath += LoseExperience;
    }

    private void OnDisable()
    {
      health.OnDeath -= LoseExperience;
    }

    private void LoseExperience()
    {
      livingEntity.LoseExperience();
    }
  }
}