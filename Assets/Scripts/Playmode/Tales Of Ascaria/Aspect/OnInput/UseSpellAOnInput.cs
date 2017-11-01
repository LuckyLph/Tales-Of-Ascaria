using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class UseSpellAOnInput : GameScript
  {
    private LivingEntity livingEntity;
    private SpellCore spellCore;
    private PlayerInput playerInput;
    private PlayerController playerController;

    private void InjectUseSpellAOnInput([GameObjectScope] LivingEntity livingEntity,
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
      InjectDependencies("InjectUseSpellAOnInput");
    }

    private void OnEnable()
    {
      playerInput.OnButtonAPressed += UseSpellA;
    }

    private void OnDisable()
    {
      playerInput.OnButtonAPressed -= UseSpellA;
    }

    private void UseSpellA()
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        Vector2 positionWithOffset = transform.parent.transform.position;
        positionWithOffset.y += playerController.PlayerSize.y / 3;
        spellCore.SpellPressedA(livingEntity.GetStats().GetStatsSnapshot(), playerController.Direction.normalized, positionWithOffset);
      }
    }
  }
}