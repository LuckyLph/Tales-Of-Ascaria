using System;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Object/Aspect/DisableSpriteOnCollision")]
  public class DisableSpriteOnCollision : GameScript
  {
    private PlayerSensor playerSensor;
    private SpriteRenderer spriteRenderer;

    private void InjectDisableSpriteOnCollision([EntityScope] PlayerSensor playerSensor)
    {
      this.playerSensor = playerSensor;
    }

    private void Awake()
    {
      InjectDependencies("InjectDisableSpriteOnCollision");

      int layer = LayerMask.NameToLayer(R.S.Layer.PlayerSensor);
      if (layer == -1)
      {
        throw new Exception("In order to use a PlayerSensor, you must have a " + R.S.Layer.PlayerSensor + " layer.");
      }
      transform.root.gameObject.layer = layer;

      spriteRenderer = gameObject.GetComponentInParent<SpriteRenderer>();
    }

    private void OnEnable()
    {
      playerSensor.OnPlayerSensorEntered += OnPlayerCollision;
    }

    private void OnDisable()
    {
      playerSensor.OnPlayerSensorEntered -= OnPlayerCollision;
    }

    private void OnPlayerCollision(GameObject entity)
    {
      spriteRenderer.enabled = false;
    }
  }
}