using System.Collections;
using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New High Guard Effect", menuName = "Game/Effect/High Guard")]
  public class HighGuard : Effect
  {
    [Tooltip("Duration of the effect in seconds")]
    [SerializeField] private int durationInSeconds;

    [Tooltip("Pourcentage de bonus de constitution (0 à 1 réduit la constitution, plus grand que 1 augmente la constitution)")]
    [SerializeField] private float constitutionFactor;

    [Tooltip("Pourcentage de réduction de dommages (0 est invincibilité)")]
    [Range(0,1)]
    [SerializeField] private float damageReduction;

    private void Start()
    {
      
    }

    public override void ApplyOn(LivingEntity entity)
    {
      entity.StartCoroutine(ApplyAlterationOn(entity.GetStats()));
    }

    private IEnumerator ApplyAlterationOn(Stats targetStats)
    {
      StatMultiplierBonus bonus = new StatMultiplierBonus(1,1,constitutionFactor,1,1,1,1,1,1,damageReduction);
      targetStats.AddMultiplier(bonus);
      yield return new WaitForSeconds(durationInSeconds);
      targetStats.RemoveMultiplier(bonus);
    }
  }
}

