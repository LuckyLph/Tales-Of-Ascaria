using System.Collections;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class PierceMoveable : Moveable
	{
	  [Tooltip("La distance auquel le visuel de l'arme sera déplacé.")]
    [SerializeField] private float weaponRange;

	  private float unitsPerSecond;

    public override void ExecuteMove()
    {
      unitsPerSecond = weaponRange / timeTaken;
      StartCoroutine(Stab());
    }

	  private IEnumerator Stab()
	  {
	    TriggerStart();
      float currentDistance = 0f;
	    while (currentDistance < weaponRange)
	    {
	      float distanceThisFrame = unitsPerSecond * Time.deltaTime;
	      transform.Translate(Vector3.up * distanceThisFrame, Space.Self);
	      currentDistance += distanceThisFrame;
	      yield return null;
	    }
      TriggerEnd();
	  }
  }
}