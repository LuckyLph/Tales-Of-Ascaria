using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Snare", menuName = "Game/Effect/Snare")]
  public class Snare : Effect, ICleansable
  {
    [Tooltip("Duration of the snare in seconds.")]
    [SerializeField] private float duration;
    public float Duration
    {
      get { return duration; }
      set { duration = value; }
    }

    private float endTime;

    public override void ApplyOn(LivingEntity entity)
    {
      entity.OnCleanse += Cleanse;
      entity.StartCoroutine(ApplySnare(entity));
    }

    private IEnumerator ApplySnare(LivingEntity entity)
    {
      endTime = Time.time + duration;
      entity.GetCrowdControl().IncreaseSnareCount();
      while (endTime >= Time.time)
      {
        yield return new WaitForEndOfFrame();
      }
      entity.GetCrowdControl().ReduceSnareCount();
    }

    public void Cleanse(LivingEntity entity)
    {
      endTime = Time.time;
      entity.OnCleanse -= Cleanse;
    }
  }
}