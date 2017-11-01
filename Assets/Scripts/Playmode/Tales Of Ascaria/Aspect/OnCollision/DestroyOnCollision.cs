using System;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Object/Aspect/DestroyOnCollision")]
  public class DestroyOnCollision : GameScript
  {
    private PlayerSensor playerSensor;
    private EntityDestroyer entityDestroyer;

    private void InjectDestroyOnCollision([EntityScope] PlayerSensor playerSensor,
                                          [EntityScope] EntityDestroyer entityDestroyer)
    {
      this.playerSensor = playerSensor;
      this.entityDestroyer = entityDestroyer;
    }

    private void Awake()
    {
      InjectDependencies("InjectDestroyOnCollision");

      int layer = LayerMask.NameToLayer(R.S.Layer.PlayerSensor);
      if (layer == -1)
      {
        throw new Exception("In order to use a HitSensor, you must have a " + R.S.Layer.PlayerSensor + " layer.");
      }
      transform.root.gameObject.layer = layer;
    }

    private void OnEnable()
    {
      playerSensor.OnPlayerSensorEntered += DestroyObject;
    }

    private void OnDisable()
    {
      playerSensor.OnPlayerSensorEntered -= DestroyObject;
    }

    private void DestroyObject(GameObject gameObject)
    {
      entityDestroyer.Destroy();
    }
  }
}