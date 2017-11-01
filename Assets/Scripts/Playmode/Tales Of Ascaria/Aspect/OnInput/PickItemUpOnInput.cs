using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class PickItemUpOnInput : GameScript
  {
    private LivingEntity livingEntity;
    private Inventory inventory;
    private PlayerInput playerInput;
    private PlayerController playerController;

    private void InjectPickItemUpOnInput([GameObjectScope] LivingEntity livingEntity,
                                         [GameObjectScope] Inventory inventory,
                                         [GameObjectScope] PlayerInput playerInput,
                                         [GameObjectScope] PlayerController playerController)
    {
      this.livingEntity = livingEntity;
      this.inventory = inventory;
      this.playerInput = playerInput;
      this.playerController = playerController;
    }

    private void Awake()
    {
      InjectDependencies("InjectPickItemUpOnInput");
    }

    private void OnEnable()
    {
      playerInput.OnButtonBPressed += PickItemUp;
    }

    private void OnDisable()
    {
      playerInput.OnButtonBPressed -= PickItemUp;
    }

    private void PickItemUp()
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        if (!playerController.InventoryView.Displayed)
        {
          inventory.Interact();
        }
      }
    }
  }
}