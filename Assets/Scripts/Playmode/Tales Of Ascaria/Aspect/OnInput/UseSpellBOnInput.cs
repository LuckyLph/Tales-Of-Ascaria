using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class UseSpellBOnInput : GameScript
  {
    private LivingEntity livingEntity;
    private SpellCore spellCore;
    private PlayerInput playerInput;
    private PlayerController playerController;

    private void InjectUseSpellBOnInput([GameObjectScope] LivingEntity livingEntity,
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
      InjectDependencies("InjectUseSpellBOnInput");
    }

    private void OnEnable()
    {
      playerInput.OnButtonBPressed += UseSpellB;
    }

    private void OnDisable()
    {
      playerInput.OnButtonBPressed -= UseSpellB;
    }

    private void UseSpellB()
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        Vector2 positionWithOffset = transform.parent.transform.position;
        positionWithOffset.y += playerController.PlayerSize.y / 3;
        spellCore.SpellPressedB(livingEntity.GetStats().GetStatsSnapshot(), playerController.Direction.normalized, positionWithOffset);
      }
    }
  }
}