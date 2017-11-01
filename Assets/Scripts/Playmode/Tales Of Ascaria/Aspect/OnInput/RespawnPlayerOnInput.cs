using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class RespawnPlayerOnInput : GameScript
  {
    public Transform RespawnPosition { get; set; }

    private PlayersList playersList;
    private PlayerRespawnEventPublisher playerRespawnEventPublisher;
    private PlayerInput playerInput;
    private ResetPlayerOnRespawn reset;

    private void InjectRespawnPlayerOnInput([ApplicationScope] PlayersList playersList,
                                            [EntityScope] PlayerRespawnEventPublisher playerRespawnEventPublisher,
                                            [GameObjectScope] PlayerInput playerInput,
                                            [GameObjectScope] ResetPlayerOnRespawn reset)
    {
      this.playersList = playersList;
      this.playerRespawnEventPublisher = playerRespawnEventPublisher;
      this.playerInput = playerInput;
      this.reset = reset;
    }

    private void Awake()
    {
      InjectDependencies("InjectRespawnPlayerOnInput");
      playerInput.OnButtonBPressed += RespawnPlayer;
    }

    private void OnDestroy()
    {
      playerInput.OnButtonBPressed -= RespawnPlayer;
    }

    private void RespawnPlayer()
    {
      if (playersList.PlayersAlive.Count == 0)
      {
        Respawn();
      }
    }

    private void Respawn()
    {
      reset.ResetPlayer();
      playersList.PlayersAlive.Add(transform.root.gameObject);
      playersList.PlayersDead.Remove(transform.root.gameObject);
      playersList.ActivePlayersAmount++;
      playerRespawnEventPublisher.Publish(playersList.Players.ToArray());
    }
  }
}