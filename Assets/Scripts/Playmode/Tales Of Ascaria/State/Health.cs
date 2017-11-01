using UnityEngine;
using System;

namespace TalesOfAscaria
{
  /// <summary>
  /// Cette classe représente un composant de vie
  /// </summary>
  public class Health : GameScript, IHealthMockable
  {
    /// delegates pour les events
    public delegate void DeathEventHandler();
    public delegate void HealthChangedHandler(float remainingHealth);

    /// <summary>
    /// Lorsque la vie tombe à 0
    /// </summary>
    public event DeathEventHandler OnDeath;

    /// <summary>
    /// Lorsque la vie est changée
    /// </summary>
    public event HealthChangedHandler OnHealthChanged;


    /// <summary>
    /// Les points de vie actuels.
    /// </summary>
    public float HealthPoints { get; private set; }

    /// <summary>
    /// Le maximum de vie
    /// </summary>
    public float MaximumHealthPoints
    {
      get { return maximumHealthPoints; }
      private set { maximumHealthPoints = value; }
    }

    [Tooltip("Entity's maximum health. Will be the entity's default health on initialization.")]
    [SerializeField]
    float maximumHealthPoints;

    /// <summary>
    /// Appelé à l'instanciation de la classe.
    /// </summary>
    public void Awake()
    {
      if (MaximumHealthPoints < 0)
      {
        MaximumHealthPoints = 0;
      }
      RegainHealth();
    }

    /// <summary>
    /// Redonne toute la vie au composant
    /// </summary>
    public void RegainHealth()
    {
      HealthPoints = MaximumHealthPoints;
      if (OnHealthChanged != null) OnHealthChanged(HealthPoints);
    }

    /// <summary>
    /// Modifie la vie maximale de l'entité, et regénère l'entité de cette augmentation
    /// </summary>
    /// <param name="healthIncrease">Points de vie augmentés</param>
    public void ChangeMaxHealth(float healthIncrease)
    {
      MaximumHealthPoints += healthIncrease;
      HealthPoints += healthIncrease;
      if (OnHealthChanged != null) OnHealthChanged(HealthPoints);
    }

    /// <summary>
    /// Soigne l'entité d'un nombre de points de vie
    /// </summary>
    /// <param name="healPoints">Nombre de points de vie à soigner.  Doit être supérieur à 0</param>
    public void Heal(float healPoints)
    {
      if (healPoints > 0)
      {
        if (HealthPoints + healPoints > MaximumHealthPoints)
        {
          RegainHealth();
        }
        else
        {
          HealthPoints += healPoints;
        }
        if (OnHealthChanged != null) OnHealthChanged(HealthPoints);
      }
    }

    /// <summary>
    /// Retire de la vie à l'entité.
    /// </summary>
    /// <param name="damage">Le dégat à faire. Doit être supérieur à 0</param>
    public void TakeDamage(float damage)
    {
      if (damage > 0)
      {
        HealthPoints = Math.Max(0, HealthPoints - damage);
        if (OnHealthChanged != null) OnHealthChanged(HealthPoints);
        if (HealthPoints <= 0)
        {
          if (OnDeath != null) OnDeath();
        }
      }
    }
  }
}