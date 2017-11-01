using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class UseWeaponSecondarySpellOnInput : GameScript
  {

    private LivingEntity livingEntity;
    private SpellCore spellCore;
    private PlayerInput playerInput;
    private PlayerController playerController;

    private void InjectUseWeaponSecondarySpellOnInput([GameObjectScope] LivingEntity livingEntity,
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
      InjectDependencies("InjectUseWeaponSecondarySpellOnInput");
    }

    private void OnEnable()
    {
      playerInput.OnButtonXPressed += UseWeaponSecondarySpell;
    }

    private void OnDisable()
    {
      playerInput.OnButtonXPressed -= UseWeaponSecondarySpell;
    }

    private void UseWeaponSecondarySpell()
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        Vector2 positionWithOffset = transform.parent.transform.position;
        positionWithOffset.y += playerController.PlayerSize.y / 3;
        spellCore.SpellPressedSecondary(livingEntity.GetStats().GetStatsSnapshot(), playerController.Direction.normalized, positionWithOffset);
      }
    }
  }
}