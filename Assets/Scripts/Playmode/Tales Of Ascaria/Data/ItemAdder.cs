using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ItemAdder : GameScript
  {
    private Inventory inventory;
    private ItemGenerator generator;
    private LivingEntity livingEntity;

    private void Awake()
    {
      InjectDependencies("InjectItemAdder");
      AddItem(0);
      AddItem(1);
      AddItem(2);
      AddItem(3);
      AddItem(4);
      AddItem(5);
      AddItem(6);
      AddItem(7);
      AddItem(8);
      AddItem(9);
      AddItem(10);
      AddItem(11);
      AddItem(12);
      AddItem(13);
      AddItem(14);
    }

    private void InjectItemAdder([EntityScope] Inventory inventory, [ApplicationScope] ItemGenerator generator,[EntityScope] LivingEntity livingEntity)
    {
      this.inventory = inventory;
      this.generator = generator;
      this.livingEntity = livingEntity;
    }

    /// <summary>
    /// Ajoute un item de niveau un
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="rarity">La rareté de l'item. Commun par défaut</param>
    /// <returns>Si l'item a bel et bien été ajouté</returns>
    public bool AddLevelOneItem(int itemID, ItemRarity rarity = ItemRarity.Common)
    {
      if (itemID < 0)
      {
        Debug.LogError("Le ID est invalide.");
      }
      return AddItem(generator.GetRandomItem(itemID, rarity, 1));
    }

    /// <summary>
    /// Ajoute un item de niveau un, avec une rareté aléatoire.
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <returns>Si l'item a bel et bien été ajouté</returns>
    public bool AddLevelOneItem(int itemID)
    {
      if (itemID < 0)
      {
        Debug.LogError("Le ID est invalide.");
      }
      return AddItem(generator.GetRandomItem(itemID, 1));
    }

    /// <summary>
    /// Ajoute un item du niveau désiré, avec une rareté aléatoire
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="level">Le niveau de l'item</param>
    /// <returns>Si l'item a bel et bien été ajouté</returns>
    public bool AddItem(int itemID, int level)
    {
      if (itemID < 0)
      {
        Debug.LogError("Le ID est invalide.");
      }
      return AddItem(generator.GetRandomItem(itemID, level));
    }

    /// <summary>
    /// Ajoute un item du niveau du joueur avec une rareté aléatoire.
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <returns>Si l'item a bel et bien été ajouté</returns>
    public bool AddItem(int itemID)
    {
      if (itemID < 0)
      {
        Debug.LogError("Le ID est invalide.");
      }
      return AddItem(itemID, livingEntity.GetLevel());
    }

    /// <summary>
    /// Ajoute un item du niveau désiré
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="level">Le niveau de l'item</param>
    /// <param name="rarity">La rareté de l'item. Commun par défaut</param>
    /// <returns>Si l'item a bel et bien été ajouté</returns>
    public bool AddItem(int itemID, int level, ItemRarity rarity = ItemRarity.Common)
    {
      if (itemID < 0)
      {
        Debug.LogError("Le ID est invalide.");
      }
      return AddItem(generator.GetRandomItem(itemID, rarity, level));
    }

    /// <summary>
    /// Ajoute un item du niveau du joueur
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="rarity">La rareté de l'item. Commun par défaut</param>
    /// <returns>Si l'item a bel et bien été ajouté</returns>
    public bool AddItem(int itemID, ItemRarity rarity = ItemRarity.Common)
    {
      if (itemID < 0)
      {
        Debug.LogError("Le ID est invalide.");
      }
      return AddItem(itemID, livingEntity.GetLevel(), rarity);
    }

    /// <summary>
    /// Ajoute un item en se basant sur une base numérique. Tente automatiquement de trouver l'icone
    /// et le lien vers son préfab dans le dossier Resources.
    /// </summary>
    /// <remarks>
    /// * La sprite sera cherchée dans le dossier Resources/ItemIcon/[le ID de l'item]
    /// * Le prefab sera cherché dans le dossier Resources/Prefabs/WeaponPrefabs/[le ID de l'item]
    /// </remarks>
    /// <param name="item">Les numériques de l'item</param>
    /// <returns>Si l'item a bel et bien été ajouté</returns>
    public bool AddItem(Item item)
    {
      if (item == null)
      {
        Debug.LogError("L'item est null");
      }
      ItemInInventory generatedItem = null;

      
      generatedItem = new ItemInInventory(generator.GetItemSprite(item.ItemID), item, generator.GetItemLink(item.ItemID),generator.GetItemEffect(item.ItemID));
      return inventory.AddItem(generatedItem);
    }

    /// <summary>
    /// Ajoute un item selon les paramètres spécifiés.
    /// </summary>
    /// <param name="item">Les numériques de l'item</param>
    /// <param name="sprite">L'icone du spell</param>
    /// <param name="prefab">Le préfab d'attaque de l'item. Null par défaut</param>
    /// <returns>Si l'item a bel et bien été ajouté</returns>
    public bool AddItem(Item item, Sprite sprite, GameObject prefab = null,Effect[] effect = null)
    {
      if (item == null)
      {
        Debug.LogError("L'item est null");
      }
      return inventory.AddItem(new ItemInInventory(sprite, item, prefab,effect));
    }
  }
}

