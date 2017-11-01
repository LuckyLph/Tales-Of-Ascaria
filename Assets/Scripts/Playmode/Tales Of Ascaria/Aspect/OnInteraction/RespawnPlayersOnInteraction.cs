using System.Collections.Generic;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class RespawnPlayersOnInteraction : GameScript
  {
    public Transform RespawnPosition { get; set; }

    private PlayerRespawnEventPublisher playerRespawnEventPublisher;
    private ShrineController shrineController;
    private PlayersList playersList;

    private void InjectRespawnPlayersOnInteraction([SiblingsScope] PlayerRespawnEventPublisher playerRespawnEventPublisher,
                                                   [GameObjectScope] ShrineController shrineController,
                                                   [ApplicationScope] PlayersList playersList)
    {
      this.playerRespawnEventPublisher = playerRespawnEventPublisher;
      this.shrineController = shrineController;
      this.playersList = playersList;
    }

    private void Awake()
    {
      InjectDependencies("InjectRespawnPlayersOnInteraction");
    }

    private void OnEnable()
    {
      shrineController.OnShrineInteractionTriggered += RespawnTeammates;
    }

    private void OnDisable()
    {
      shrineController.OnShrineInteractionTriggered -= RespawnTeammates;
    }

    private void RespawnTeammates(XInputDotNetPure.PlayerIndex playerIndex)
    {
      Respawn(playersList.PlayersDead);
    }

    private void Respawn(List<GameObject> players)
    {
      for (int i = 0; i < players.Count; i++)
      {
        players[i].GetComponentInChildren<ResetPlayerOnRespawn>().ResetPlayer();
        playersList.PlayersAlive.Add(players[i]);
        playersList.PlayersDead.Remove(players[i]);
        playersList.ActivePlayersAmount++;
      }
      playerRespawnEventPublisher.Publish(players.ToArray());
    }
  }
}