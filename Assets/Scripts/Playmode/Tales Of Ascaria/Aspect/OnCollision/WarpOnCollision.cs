using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void WarpTriggerHandler();

  public class WarpOnCollision : GameScript
  {
    [SerializeField]
    private Transform warpTarget;

    [SerializeField]
    private bool mustWarpEveryPlayer;

    [SerializeField]
    private bool mustWarpCamera;

    private ScreenFader screenFader;
    private GameObject playerToWarp;
    private PlayersList playersList;
    private PlayerWarpEventPublisher playerWarpEventPublisher;
    private new Camera camera;
    private PlayerSensor playerSensor;

    private void InjectWarpOnCollision([SceneScope] ScreenFader screenFader,
                                      [ApplicationScope] PlayersList playersList,
                                      [EntityScope] PlayerWarpEventPublisher playerWarpEventPublisher,
                                      [TagScope(R.S.Tag.MainCamera)] Camera camera,
                                      [GameObjectScope] PlayerSensor playerSensor)
    {
      this.screenFader = screenFader;
      this.playersList = playersList;
      this.playerWarpEventPublisher = playerWarpEventPublisher;
      this.camera = camera;
      this.playerSensor = playerSensor;
    }

    private void Awake()
    {
      InjectDependencies("InjectWarpOnCollision");

      playerSensor.OnPlayerSensorEntered += OnPlayerSensorTriggered;
    }

    private void OnDestroy()
    {
      playerSensor.OnPlayerSensorEntered -= OnPlayerSensorTriggered;
    }

    public void TriggerWarp()
    {
      GameObject[] players = playersList.PlayersAlive.ToArray();
      if (mustWarpEveryPlayer)
      {
        for (int i = 0; i < players.Length; i++)
        {
          players[i].transform.position = warpTarget.position;
        }

        playerWarpEventPublisher.Publish(players, gameObject, warpTarget.gameObject);
      }
      else
      {
        for (int i = 0; i < players.Length; i++)
        {
          if (players[i] == playerToWarp)
          {
            playerToWarp.transform.position = warpTarget.position;
            playerWarpEventPublisher.Publish(new GameObject[] { playerToWarp }, gameObject, warpTarget.gameObject);
          }
        }
      }
      if (mustWarpCamera)
      {
        camera.transform.position = warpTarget.position + new Vector3(0, 0, camera.transform.position.z);
      }
    }

    private void OnPlayerSensorTriggered(GameObject player)
    {
      playerToWarp = player;
      screenFader.FadeOutAndIn(TriggerWarp);
    }
  }
}