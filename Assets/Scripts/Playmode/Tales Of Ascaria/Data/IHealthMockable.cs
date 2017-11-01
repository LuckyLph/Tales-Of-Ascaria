namespace TalesOfAscaria
{
  public interface IHealthMockable
  {
    /// <summary>
    /// Lorsque la vie tombe à 0
    /// </summary>
    event Health.DeathEventHandler OnDeath;

    /// <summary>
    /// Lorsque la vie est changée
    /// </summary>
    event Health.HealthChangedHandler OnHealthChanged;

    /// <summary>
    /// Les points de vue actuels.
    /// </summary>
    float HealthPoints { get; }

    /// <summary>
    /// Le maximum de vie
    /// </summary>
    float MaximumHealthPoints { get; }


    void Awake();


    /// <summary>
    /// Redonne toute la vie au composant
    /// </summary>
    void RegainHealth();


    /// <summary>
    /// Modifie la vie maximale de l'entité, et regénère l'entité de cette augmentation
    /// </summary>
    /// <param name="healthIncrease">Points de vie augmentés</param>
    void ChangeMaxHealth(float healthIncrease);


    void Heal(float healPoints);


    /// <summary>
    /// Retire de la vie à l'entité
    /// </summary>
    /// <param name="damage">Le dégat à faire</param>
    void TakeDamage(float damage);
  }
}