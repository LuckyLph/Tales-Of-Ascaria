using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class WaspEggController : GameScript
	{
	  private EntityDestroyer entityDestroyer;
	  private PlayerEnterZoneSensor playerEnterZoneSensor;
	  private ObjectSpawner objectSpawner;

		private void Awake() 
		{
		  InjectDependencies("InjectWaspEggController");
		  playerEnterZoneSensor.OnTriggered += HandleTrigger;
		}

	  private void InjectWaspEggController([EntityScope] PlayerEnterZoneSensor playerEnterZoneSensor,
      [EntityScope] EntityDestroyer entityDestroyer, [EntityScope] ObjectSpawner objectSpawner)
	  {
	    this.playerEnterZoneSensor = playerEnterZoneSensor;
	    this.entityDestroyer = entityDestroyer;
	    this.objectSpawner = objectSpawner;
	  }

	  private void HandleTrigger(Collider2D trigger)
	  {
      objectSpawner.SpawnObjects();
	    entityDestroyer.Destroy();
	  }
	}
}