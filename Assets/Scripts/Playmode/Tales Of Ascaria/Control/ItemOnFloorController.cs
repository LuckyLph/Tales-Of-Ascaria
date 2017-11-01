using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Représente un item au sol
  /// </summary>
  /// <remarks>
  /// A.Gagné
  /// </remarks>
  public class ItemOnFloorController : GameScript
  {

    private Item containedItem;
    private ItemGenerator generator;

    private new SpriteRenderer renderer;


    /// <summary>
    /// L'item contenu dans ce Wrapper
    /// </summary>
    public Item Item
    {
      get { return containedItem; }
      set { containedItem = value; }
    }

    private void Awake()
    {
      InjectDependencies("InjectItemOnFloorController");
    }

    private void Start()
    {
      //Mettre la sprite. On ne le fait pas au Awake puisqu'il faut attendre que l'item soit injecté
      renderer.sprite = generator.GetItemSprite(Item.ItemID);
      renderer.color = Item.GetSpriteColorBasedOnRarity(containedItem.Rarity);
    }

    private void InjectItemOnFloorController([EntityScope] SpriteRenderer renderer, [ApplicationScope] ItemGenerator generator)
    {
      this.renderer = renderer;
      this.generator = generator;
    }

    /// <summary>
    /// Ajoute l'item à l'inventaire, puis le détruit
    /// </summary>
    /// <param name="adder">L'inventaire pour ajouter l'item</param>
    /// <param name="deleteItemAfter">Supprime l'item après l'ajout si vrai</param>
    public void PickUp(ItemAdder adder, bool deleteItemAfter = true)
    {
      if(deleteItemAfter && adder.AddItem(containedItem)) DeleteItem();
    }
    /// <summary>
    /// Ajoute l'item à tous les inventaires spécifiés
    /// </summary>
    /// <param name="adders">Les inventaires</param>
    public void PickUp(ItemAdder[] adders)
    {
      for (int i = 0; i < adders.Length; i++)
      {
        PickUp(adders[i],false);
      }
      DeleteItem();
    }

    /// <summary>
    /// Supprime le gameobject, afin d'empêcher que l'item soit ramassé encore
    /// </summary>
    void DeleteItem()
    {
      Destroy(gameObject);
    }

    
  }
}


