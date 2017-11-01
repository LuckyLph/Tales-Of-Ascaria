using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Lifesteal", menuName = "Game/Effect/Lifesteal")]
  public class Lifesteal : Effect
  {
    [Tooltip("Percentage of the lifesteal.")]
    [SerializeField]
    private float lifestealPercentage;

    public float LifestealPercentage
    {
      get { return lifestealPercentage; }
      set { lifestealPercentage = value; }
    }

    public LivingEntity CastingEntity { get; set; }

    public float DamageDealt { get; set; }

    public override void ApplyOn(LivingEntity entity)
    {
      CastingEntity.Heal(DamageDealt * lifestealPercentage);
    }
  }
}