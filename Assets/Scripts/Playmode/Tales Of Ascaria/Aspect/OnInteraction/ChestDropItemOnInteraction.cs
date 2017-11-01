using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ChestDropItemOnInteraction : GameScript
  {
    [Tooltip("Item drop position")]
    [SerializeField]
    private Transform itemTransform;

    private ChestController chestController;
    private ItemGenerator itemGenerator;

    private void InjectChestDropItemOnInteraction([GameObjectScope] ChestController chestController,
                                                  [ApplicationScope] ItemGenerator itemGenerator)
    {
      this.chestController = chestController;
      this.itemGenerator = itemGenerator;
    }

    private void Awake()
    {
      InjectDependencies("InjectChestDropItemOnInteraction");
    }

    private void OnEnable()
    {
      chestController.OnChestInteractionSensorTriggered += OnInteractionSensorTriggered;
    }

    private void OnDisable()
    {
      chestController.OnChestInteractionSensorTriggered -= OnInteractionSensorTriggered;
    }
    
    private void OnInteractionSensorTriggered(XInputDotNetPure.PlayerIndex playerIndex)
    {
      if (!chestController.IsOpen)
      {
        Item item = itemGenerator.GetRandomItem(Random.Range(0, itemGenerator.GetMaximumItemID()), GetPlayerLevel());
        item.Drop(itemTransform.position);
        chestController.IsOpen = true;
      }
    }

    private int GetPlayerLevel()
    {
      int playerID = (int)chestController.PlayerIndex + 1;
      GameObject player = GameObject.FindGameObjectWithTag(R.S.Tag.Player + playerID);
      if (player != null)
      {
        LivingEntity entity = player.GetComponentInChildren<LivingEntity>();
        if (entity != null)
        {
          return entity.GetLevel();
        }
      }
      return -1;
    }
  }
}