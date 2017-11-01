using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Instant Damage Effect", menuName = "Game/Effect/Instant Damage")]
  public class InstantDamage : Effect
  {
    [Tooltip("Base damage points. Total damage can be affected by various spell parameters during the cast.")]
    [SerializeField]
    private float baseDamagePoints;

    [Tooltip("Type of damage. True damage will ignore constitution and spirit.")]
    [SerializeField]
    private DamageType damageType;

    //Dégâts bonus, calculés dans le cast de la spell si nécessaire
    public float BonusDamage { get; set; }

    public override void ApplyOn(LivingEntity entity)
    {
      float damage = baseDamagePoints + BonusDamage;
      if (damageType == DamageType.True)
      {
        entity.TakeUnreductibleDamage(damage);
      }
      else
      {
        entity.TakeReductibleDamage(damage, damageType);
      }
    }
  }
}