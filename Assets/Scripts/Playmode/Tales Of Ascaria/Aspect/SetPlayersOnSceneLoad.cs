using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class SetPlayersOnSceneLoad : GameScript
  {
    [Tooltip("The players' spawn positions")]
    [SerializeField]
    private Transform[] spawnPositions;

    [Tooltip("The camera's spawn position")]
    [SerializeField]
    private Transform cameraPosition;

    private PlayersList playersList;
    private ShrineController shrineController;
    private SceneSwitchedEventPublisher sceneSwitchedEventPublisher;
    private new GameObject camera;

    private void InjectSetPlayerPositionOnSceneLoad([ApplicationScope] PlayersList playersList,
                                                    [TagScope(R.S.Tag.DefaultRespawnPoint)] ShrineController shrineController,
                                                    [SiblingsScope] SceneSwitchedEventPublisher sceneSwitchedEventPublisher,
                                                    [TagScope(R.S.Tag.MainCamera)] GameObject camera)
    {
      this.playersList = playersList;
      this.shrineController = shrineController;
      this.sceneSwitchedEventPublisher = sceneSwitchedEventPublisher;
      this.camera = camera;
    }

    private void Awake()
    {
      InjectDependencies("InjectSetPlayerPositionOnSceneLoad");
    }

    private void Start()
    {
      GameObject[] players = playersList.Players.ToArray();
      for (int i = 0; i < players.Length; i++)
      {
        players[i].transform.position = spawnPositions[i].position;
        players[i].GetComponentInChildren<PlayerController>().RespawnPosition = shrineController.PlayersRespawnPositions[i];
      }
      sceneSwitchedEventPublisher.Publish();
      camera.transform.position = cameraPosition.position + new Vector3(0, 0, -10);
    }
  }
}