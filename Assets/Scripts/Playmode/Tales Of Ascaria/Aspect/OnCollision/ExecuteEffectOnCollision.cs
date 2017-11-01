using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Object/Aspect/ExecuteEffectOnCollision")]
  public class ExecuteEffectOnCollision : GameScript
  {
    [SerializeField]
    private List<Effect> effects;

    private PlayerSensor playerSensor;

    private void InjectExecuteEffectOnCollision([GameObjectScope] PlayerSensor playerSensor)
    {
      this.playerSensor = playerSensor;
    }

    private void Awake()
    {
      InjectDependencies("InjectExecuteEffectOnCollision");
    }

    private void OnEnable()
    {
      playerSensor.OnPlayerSensorEntered += OnCollisionActivate;
    }

    private void OnDisable()
    {
      playerSensor.OnPlayerSensorEntered -= OnCollisionActivate;
    }

    private void OnCollisionActivate(GameObject player)
    {
      for (int i = 0; i < effects.Count; i++)
      {
        if (effects[i].GetType() == typeof(Knockback))
        {
          Knockback knockback = effects[i] as Knockback;
          knockback.KnockbackSourceDirection = transform;
          effects[i] = knockback;
        }
        effects[i].ApplyOn(player.GetComponentInChildren<LivingEntity>());
      }
    }
  }
}