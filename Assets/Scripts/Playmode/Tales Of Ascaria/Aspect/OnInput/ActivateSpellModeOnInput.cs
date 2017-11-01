using Harmony;

namespace TalesOfAscaria
{
  public class ActivateSpellModeOnInput : GameScript
  {
    private LivingEntity livingEntity;
    private SpellCore spellCore;
    private PlayerInput playerInput;

    private void InjectActivateSpellModeOnInput([GameObjectScope] LivingEntity livingEntity,
                                                [GameObjectScope] SpellCore spellCore,
                                                [GameObjectScope] PlayerInput playerInput)
    {
      this.livingEntity = livingEntity;
      this.spellCore = spellCore;
      this.playerInput = playerInput;
    }

    private void Awake()
    {
      InjectDependencies("InjectActivateSpellModeOnInput");
    }

    private void OnEnable()
    {
      playerInput.OnSpellMode += OnSpellMode;
    }

    private void OnDisable()
    {
      playerInput.OnSpellMode -= OnSpellMode;
    }

    private void OnSpellMode()
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        spellCore.HandleSpellButton();
      }
    }
  }
}