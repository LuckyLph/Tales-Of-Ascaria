using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class SwitchSceneOnCollision : GameScript
  {
    [SerializeField]
    private Activity nextActivity;

    private ActivityStack activityStack;
    private SceneSwitchedEventPublisher sceneSwitchedEventPublisher;
    private new CameraController camera;
    private PlayerSensor playerSensor;

    private void InjectSwitchSceneOnCollision([ApplicationScope] ActivityStack activityStack,
                                              [TagScope(R.S.Tag.MainCamera)] CameraController camera,
                                              [ParentScope] SceneSwitchedEventPublisher sceneSwitchedEventPublisher,
                                              [GameObjectScope] PlayerSensor playerSensor)
    {
      this.activityStack = activityStack;
      this.camera = camera;
      this.sceneSwitchedEventPublisher = sceneSwitchedEventPublisher;
      this.playerSensor = playerSensor;
    }

    private void Awake()
    {
      InjectDependencies("InjectSwitchSceneOnCollision");
    }

    private void OnEnable()
    {
      playerSensor.OnPlayerSensorEntered += SwitchScene;
    }
    

    private void OnDisable()
    {
      playerSensor.OnPlayerSensorEntered -= SwitchScene;
    }

    private void SwitchScene(GameObject player)
    {
      player.GetComponentInChildren<InteractionStimulus>().ResetSensors();
      camera.HasMapBounds = false;
      sceneSwitchedEventPublisher.Publish();
      activityStack.ChangeCurrentActivity(nextActivity);
    }
  }
}