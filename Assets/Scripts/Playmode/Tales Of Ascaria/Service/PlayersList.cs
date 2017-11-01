using UnityEngine;
using System.Collections.Generic;

namespace TalesOfAscaria
{
  public class PlayersList : GameScript
  {
    public int ActivePlayersAmount { get; set; }
    public List<GameObject> PlayersAlive { get; private set; }
    public List<GameObject> PlayersDead { get; private set; }
    public List<GameObject> Players { get; private set; }
    public List<SpriteRenderer> SpriteRenderers { get; set; }

    private void Awake()
    {
      PlayersAlive = new List<GameObject>();
      PlayersDead = new List<GameObject>();
      Players = new List<GameObject>();
      SpriteRenderers = new List<SpriteRenderer>();
    }

    private void OnDestroy()
    {
      for (int i = 0; i < ActivePlayersAmount; i++)
      {
        if (Players[i] !=  null)
        {
          Players[i].GetComponentInChildren<PlayerInput>().OnPlayerDeath -= OnPlayerDeath;
        }
      }
    }

    private void OnPlayerDeath(XInputDotNetPure.PlayerIndex playerIndex)
    {
      PlayersAlive.Remove(Players[(int)playerIndex]);
      PlayersDead.Add(Players[(int)playerIndex]);
      ActivePlayersAmount--;
    }

    public void SetPlayers(GameObject player, SpriteRenderer spritesRenderer)
    {
      Players.Add(player);
      SpriteRenderers.Add(spritesRenderer);
      ActivePlayersAmount = Players.Count;
      for (int i = 0; i < ActivePlayersAmount; i++)
      {
        PlayersAlive.Add(Players[i]);
      }
      for (int j = 0; j < ActivePlayersAmount; j++)
      {
        Players[j].GetComponentInChildren<PlayerInput>().OnPlayerDeath += OnPlayerDeath;
      }
    }
  }
}