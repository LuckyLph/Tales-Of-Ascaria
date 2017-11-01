using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class UseSpellYOnInput : GameScript
  {
    private LivingEntity livingEntity;
    private SpellCore spellCore;
    private PlayerInput playerInput;
    private PlayerController playerController;

    private void InjectUseSpellYOnInput([GameObjectScope] LivingEntity livingEntity,
                                        [GameObjectScope] SpellCore spellCore,
                                        [GameObjectScope] PlayerInput playerInput,
                                        [GameObjectScope] PlayerController playerController)
    {
      this.livingEntity = livingEntity;
      this.spellCore = spellCore;
      this.playerInput = playerInput;
      this.playerController = playerController;
    }

    private void Awake()
    {
      InjectDependencies("InjectUseSpellYOnInput");
    }

    private void OnEnable()
    {
      playerInput.OnButtonYPressed += UseSpellY; 
    }

    private void OnDisable()
    {
      playerInput.OnButtonYPressed -= UseSpellY;
    }

    private void UseSpellY()
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        Vector2 positionWithOffset = transform.parent.transform.position;
        positionWithOffset.y += playerController.PlayerSize.y / 3;
        spellCore.SpellPressedY(livingEntity.GetStats().GetStatsSnapshot(), playerController.Direction.normalized, positionWithOffset);
      }
    }
  }
}