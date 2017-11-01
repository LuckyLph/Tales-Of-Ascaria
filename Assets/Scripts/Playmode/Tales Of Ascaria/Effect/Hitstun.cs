using System.Collections;
using UnityEngine;

namespace TalesOfAscaria
{
  public class Hitstun : Effect
  {
    [Tooltip("Duration of the hitstun in seconds.")]
    [SerializeField]
    private float duration;


    public override void ApplyOn(LivingEntity entity)
    {
      entity.StartCoroutine(ApplyHitstun(entity));
    }


    public IEnumerator ApplyHitstun(LivingEntity entity)
    {
      yield return null;
    }
  }
}