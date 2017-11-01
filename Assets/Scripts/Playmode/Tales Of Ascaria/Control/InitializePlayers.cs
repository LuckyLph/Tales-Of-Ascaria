using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  //Classe temporaire puisque le chargement des joueurs n'est pas encore fait
  public class InitializePlayers : GameScript
  {
    private PlayersList playersList;
    private SpriteRenderer spriteRenderer;
    private PlayerInitializedEventPublisher eventPublisher;
    //private GameObject player1;
    //private GameObject player2;
    //private GameObject player3;
    //private GameObject player4;
    //private SpriteRenderer renderer1;
    //private SpriteRenderer renderer2;
    //private SpriteRenderer renderer3;
    //private SpriteRenderer renderer4;

    private void InjectInitializePlayers([ApplicationScope] PlayersList playersList,
                                         [EntityScope] SpriteRenderer spriteRenderer,
                                         [EntityScope] PlayerInitializedEventPublisher eventPublisher/*
                                         [TagScope(R.S.Tag.Player1)] GameObject player1,
                                         [TagScope(R.S.Tag.Player2)] GameObject player2,
                                         [TagScope(R.S.Tag.Player3)] GameObject player3,
                                         [TagScope(R.S.Tag.Player4)] GameObject player4*/)
    {
      this.playersList = playersList;
      this.spriteRenderer = spriteRenderer;
      this.eventPublisher = eventPublisher;
      //this.player = player1;
      //this.player2 = player2;
      //this.player3 = player3;
      //this.player4 = player4;

      //renderer1 = player1.GetComponentInChildren<SpriteRenderer>();
      //renderer2 = player1.GetComponentInChildren<SpriteRenderer>();
      //renderer3 = player1.GetComponentInChildren<SpriteRenderer>();
      //renderer4 = player1.GetComponentInChildren<SpriteRenderer>();
    }

    private void Awake()
    {
      InjectDependencies("InjectInitializePlayers");
      GameObject player = transform.root.gameObject;
      playersList.SetPlayers(player, spriteRenderer);
      eventPublisher.Publish(player);
    }
  }
}