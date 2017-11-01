using UnityEngine;
using Harmony;
using XInputDotNetPure;
using System.Collections;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Object/Control/PlayerController")]
  public class PlayerController : GameScript
  {
    public PlayerIndex Index
    {
      get { return index; }
      set { index = value; }
    }
    public Transform RespawnPosition { get; set; }
    public bool IsDead { get; set; }
    public Vector2 Direction { get; set; }
    public Vector2 PlayerSize { get; set; }
    public InventoryView InventoryView { get; set; }

    [SerializeField]
    private PlayerIndex index;

    private SpriteRenderer spriteRenderer;
   
    public void InjectPlayerController([EntityScope] SpriteRenderer spriteRenderer)
    {
      this.spriteRenderer = spriteRenderer;
    }

    private void Awake()
    {
      InjectDependencies("InjectPlayerController");

      PlayerSize = spriteRenderer.bounds.size;
      Direction = Vector2.down;
    }
  }
}