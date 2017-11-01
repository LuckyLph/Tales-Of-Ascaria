using System.Collections;
using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Dash Effect", menuName = "Game/Effect/Dash")]
  public class Dash : Effect
  {
    [Tooltip("Speed of the dash")]
    [SerializeField]
    private float dashSpeed;

    [Tooltip("Duration of the dash in seconds")]
    [SerializeField]
    private float dashDuration;

    public Vector3 DashDirection { get; set; }
    private float endTime;

    public override void ApplyOn(LivingEntity entity)
    {
      entity.StartCoroutine(ApplyDash(entity));
    }

    public IEnumerator ApplyDash(LivingEntity entity)
    {
      Debug.Log("Starting coroutine!");
      endTime = Time.time + dashDuration;
      while (Time.time < endTime)
      {
        Vector3 movement = Vector3.MoveTowards(entity.transform.root.position, entity.transform.root.position + DashDirection, dashSpeed * Time.deltaTime);
        entity.transform.root.position = movement;
        yield return new WaitForEndOfFrame();
      }
    }


    public void StopDash()
    {
      endTime = Time.time;
    }
  }
}