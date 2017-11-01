using UnityEngine;

namespace TalesOfAscaria
{
  public class ItemOnFloorSpawner : GameScript
  {
    [Tooltip("Le préfab blank d'un item au sol.")]
    [SerializeField] private GameObject ItemOnFloorPrefab;

    public void Spawn(Item item, Vector3 position)
    {
      if (ItemOnFloorPrefab != null)
      {
        ItemOnFloorController itemOnFloor = Instantiate(ItemOnFloorPrefab, position, Quaternion.identity)
          .GetComponent<ItemOnFloorController>();
        itemOnFloor.Item = item;
      }
      else
      {
        Debug.LogError("Le préfab n'est pas réglé");
      }
    }
  }

}