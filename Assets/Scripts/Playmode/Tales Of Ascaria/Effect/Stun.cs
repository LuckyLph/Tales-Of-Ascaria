using System.Collections;
using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Stun", menuName = "Game/Effect/Stun")]
  public class Stun : Effect, ICleansable
  {
    [Tooltip("Duration of the stun in seconds.")]
    [SerializeField]
    private float duration;
    public float Duration
    {
      get { return duration; }
      set { duration = value; }
    }

    private float endTime;

    public override void ApplyOn(LivingEntity entity)
    {
      entity.OnCleanse += Cleanse;
      entity.StartCoroutine(ApplyStun(entity));
    }


    private IEnumerator ApplyStun(LivingEntity entity)
    {
      endTime = Time.time + duration;
      entity.GetCrowdControl().IncreaseStunCount();
      while (endTime >= Time.time)
      {
        yield return new WaitForEndOfFrame();
      }
      entity.GetCrowdControl().ReduceStunCount();
    }


    public void Cleanse(LivingEntity entity)
    {
      endTime = Time.time;
      entity.OnCleanse -= Cleanse;
    }
  }
}

