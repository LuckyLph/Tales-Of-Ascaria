using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class DoorController : GameScript
  {
    private PlayerEnterZoneSensor playerEnterZoneSensor;
    private SceneSwitcher sceneSwitcher;

    private void Awake()
    {
      InjectDependencies("InjectDoorController");
      playerEnterZoneSensor.OnTriggered += HandleTrigger;
    }

    private void InjectDoorController([EntityScope] PlayerEnterZoneSensor playerEnterZoneSensor,
       [EntityScope] SceneSwitcher sceneSwitcher)
    {
      this.playerEnterZoneSensor = playerEnterZoneSensor;
      this.sceneSwitcher = sceneSwitcher;
    }

    private void HandleTrigger(Collider2D trigger)
    {
      sceneSwitcher.SwitchScene();
    }
  }
}