using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class PlayersCenterOfMass : GameScript
  {
    public Vector2 CenterOfMass { get; private set; }

    private PlayerInitializedEventChannel playerInitializedEventChannel;
    private PlayerMovementEventChannel playerMovementEventChannel;
    private PlayerRespawnEventChannel playerRespawnEventChannel;
    private PlayerWarpEventChannel playerWarpEventChannel;
    private SceneSwitchedEventChannel sceneSwitchedEventChannel;
    private PlayersList playersList;

    public void InjectPlayersCenterOfMass([EventChannelScope] PlayerInitializedEventChannel playerInitializedEventChannel,
                                          [EventChannelScope] PlayerMovementEventChannel playerMovementEventChannel,
                                          [EventChannelScope] PlayerRespawnEventChannel playerRespawnEventChannel,
                                          [EventChannelScope] PlayerWarpEventChannel playerWarpEventChannel,
                                          [EventChannelScope] SceneSwitchedEventChannel sceneSwitchedEventChannel,
                                          [ApplicationScope] PlayersList playersList)
    {
      this.playerInitializedEventChannel = playerInitializedEventChannel;
      this.playerMovementEventChannel = playerMovementEventChannel;
      this.playerRespawnEventChannel = playerRespawnEventChannel;
      this.playerWarpEventChannel = playerWarpEventChannel;
      this.playersList = playersList;
      this.sceneSwitchedEventChannel = sceneSwitchedEventChannel;
    }

    public void Awake()
    {
      InjectDependencies("InjectPlayersCenterOfMass");

      playerInitializedEventChannel.OnEventPublished += OnPlayerInitialized;
      playerMovementEventChannel.OnEventPublished += OnPlayerMoved;
      playerRespawnEventChannel.OnEventPublished += OnPlayerRespawn;
      playerWarpEventChannel.OnEventPublished += OnPlayerWarped;
      sceneSwitchedEventChannel.OnEventPublished += OnSceneSwitched;
    }

    public void OnDestroy()
    {
      playerInitializedEventChannel.OnEventPublished -= OnPlayerInitialized;
      playerMovementEventChannel.OnEventPublished -= OnPlayerMoved;
      playerRespawnEventChannel.OnEventPublished -= OnPlayerRespawn;
      playerWarpEventChannel.OnEventPublished -= OnPlayerWarped;
      sceneSwitchedEventChannel.OnEventPublished -= OnSceneSwitched;
    }

    public void Start()
    {
      SetCenterOfMass();
    }

    private void OnPlayerInitialized(PlayerInitializedEvent newEvent)
    {
      SetCenterOfMass();
    }

    private void OnPlayerMoved(PlayerMovementEvent newEvent)
    {
      SetCenterOfMass();
    }

    private void OnPlayerRespawn(PlayerRespawnEvent newEvent)
    {
      SetCenterOfMass();
    }

    private void OnPlayerWarped(PlayerWarpEvent newEvent)
    {
      SetCenterOfMass();
    }

    private void OnSceneSwitched(SceneSwitchedEvent newEvent)
    {
      SetCenterOfMass();
    }

    private void SetCenterOfMass()
    {
      GameObject[] players = playersList.PlayersAlive.ToArray();
      Vector2 playerMass = new Vector2();
      for (int i = 0; i < players.Length; i++)
      {
        playerMass += new Vector2(players[i].transform.position.x, players[i].transform.position.y);
      }
      playerMass /= players.Length;
      CenterOfMass = playerMass;
    }
  }
}
