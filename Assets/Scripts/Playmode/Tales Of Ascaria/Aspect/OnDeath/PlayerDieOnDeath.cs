using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class PlayerDieOnDeath : GameScript
  {
    private readonly Vector2 deathPosition = new Vector2(0, -50);
    
    private Health health;

    private void InjectPlayerDieOnDeath([GameObjectScope] Health health)
    {
      this.health = health;
    }

    private void Awake()
    {
      InjectDependencies("InjectPlayerDieOnDeath");
    }

    private void OnEnable()
    {
      health.OnDeath += PlayerDie;
    }

    private void OnDisable()
    {
      health.OnDeath -= PlayerDie;
    }

    private void PlayerDie()
    {
      transform.parent.position = deathPosition;
      transform.root.gameObject.SetActive(false);
    }
  }
}