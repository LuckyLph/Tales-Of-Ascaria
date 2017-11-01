using System.Collections;
using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Knockback", menuName = "Game/Effect/Knockback")]
  public class Knockback : Effect
  {
    [Tooltip("Puissance du knockback en unité/seconde")]
    [SerializeField] private float knockbackPower;

    [Tooltip("Temps de repoussement")]
    [SerializeField] private float knockbackTime;

    /// <summary>
    /// Position auquel le knockback doit éloigner l'entité.
    /// </summary>
    public Transform KnockbackSourceDirection { get; set; }

    public override void ApplyOn(LivingEntity entity)
    {
      entity.StartCoroutine(ApplyAlterationOn(entity));
    }

    public IEnumerator ApplyAlterationOn(LivingEntity entity)
    {
      float currentTime = Time.time;
      //On crée une copie de la position de la source du knockback lors du knockback
      //de sorte que le knockback ne soit pas une courbe
      Vector3 impactMomentPosition = KnockbackSourceDirection.position;
      while (Time.time < currentTime + knockbackTime)
      {
        Vector3 movement =
          Vector3.MoveTowards(entity.transform.parent.position, impactMomentPosition, -1f * knockbackPower * Time.deltaTime);
        entity.transform.parent.position = movement;
        yield return new WaitForEndOfFrame();
      }
    }
  }
}

