using UnityEngine;
using System.Collections.Generic;
using Harmony;

namespace TalesOfAscaria
{
  public class SetRespawnPositionOnCollision : GameScript
  {
    private PlayersList playersList;
    private PlayerSensor playerSensor;
    private ShrineController shrineController;

    private void InjectSetRespawnPositionOnCollision([ApplicationScope] PlayersList playersList,
                                                     [SiblingsScope] PlayerSensor playerSensor,
                                                     [SiblingsScope] ShrineController shrineController)
    {
      this.playersList = playersList;
      this.playerSensor = playerSensor;
      this.shrineController = shrineController;
    }

    private void Awake()
    {
      InjectDependencies("InjectSetRespawnPositionOnCollision");
      playerSensor.OnPlayerSensorEntered += OnPlayerSensorTriggered;
    }

    private void OnDestroy()
    {
      playerSensor.OnPlayerSensorEntered -= OnPlayerSensorTriggered;
    }

    private void OnPlayerSensorTriggered(GameObject gameObject)
    {
      SetPlayerRespawnPosition();
    }

    private void SetPlayerRespawnPosition()
    {
      List<RespawnPlayerOnInput> respawnInput = new List<RespawnPlayerOnInput>();
      for (int i = 0; i < playersList.Players.Count; i++)
      {
        respawnInput.Add(playersList.Players[i].GetComponentInChildren<RespawnPlayerOnInput>());
        respawnInput[i].RespawnPosition = shrineController.PlayersRespawnPositions[i];
      }
    }
  }
}
