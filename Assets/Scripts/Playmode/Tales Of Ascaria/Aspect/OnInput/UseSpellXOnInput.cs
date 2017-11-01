using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class UseSpellXOnInput : GameScript
  {
    private LivingEntity livingEntity;
    private SpellCore spellCore;
    private PlayerInput playerInput;
    private PlayerController playerController;

    private void InjectUseSpellXOnInput([GameObjectScope] LivingEntity livingEntity,
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
      InjectDependencies("InjectUseSpellXOnInput");
    }

    private void OnEnable()
    {
      playerInput.OnButtonXPressed += UseSpellX;
    }

    private void OnDisable()
    {
      playerInput.OnButtonXPressed -= UseSpellX;
    }

    private void UseSpellX()
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        Vector2 positionWithOffset = transform.parent.transform.position;
        positionWithOffset.y += playerController.PlayerSize.y / 3;
        spellCore.SpellPressedX(livingEntity.GetStats().GetStatsSnapshot(), playerController.Direction.normalized, positionWithOffset);
      }
    }
  }
}