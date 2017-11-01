using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class UseWeaponPrimarySpellOnInput : GameScript
  {
    private LivingEntity livingEntity;
    private SpellCore spellCore;
    private PlayerInput playerInput;
    private PlayerController playerController;

    private void InjectUseWeaponPrimarySpellOnInput([GameObjectScope] LivingEntity livingEntity,
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
      InjectDependencies("InjectUseWeaponPrimarySpellOnInput");
    }

    private void OnEnable()
    {
      playerInput.OnButtonAPressed += UseWeaponPrimarySpell;
    }

    private void OnDisable()
    {
      playerInput.OnButtonAPressed -= UseWeaponPrimarySpell;
    }

    private void UseWeaponPrimarySpell()
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        Vector2 positionWithOffset = transform.parent.transform.position;
        positionWithOffset.y += playerController.PlayerSize.y / 3;
        spellCore.SpellPressedPrimary(livingEntity.GetStats().GetStatsSnapshot(), playerController.Direction.normalized, positionWithOffset);
      }
    }
  }
}