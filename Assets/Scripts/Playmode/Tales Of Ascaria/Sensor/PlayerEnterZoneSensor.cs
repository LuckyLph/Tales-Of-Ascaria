using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
  [AddComponentMenu("Game/Sensor/PlayerEnterZoneSensor")]
  public class PlayerEnterZoneSensor : GameScript
	{
	  public delegate void TriggerEventHandler(Collider2D trigger);

	  public event TriggerEventHandler OnTriggered;

	  private void OnTriggerEnter2D(Collider2D collider2D)
	  {
	    if (OnTriggered != null) OnTriggered(collider2D);
	  }
	}
}