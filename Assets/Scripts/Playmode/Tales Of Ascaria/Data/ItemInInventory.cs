using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Représente un item dans l'inventaire. Ce dernier contient des valeurs non-sérializables (non sauvegardées
  /// dans la BD) utiles au jeu.
  /// </summary>
  /// <remarks>
  /// Cloner une instance de cette classe CLONERA AUSSI l'item qui lui est associé.
  ///
  /// A.Gagné
  /// </remarks>
  public sealed class ItemInInventory : ICloneable
  {
    /// <summary>
    /// Le sprite de l'item
    /// </summary>
    public readonly Sprite Sprite;

    /// <summary>
    /// Les numériques de l'item. Unique à cet instance
    /// </summary>
    public readonly Item Item;

    /// <summary>
    /// Le préfab à instancier si l'on souhaite attaquer. SERA NUL POUR TOUT CE QUI N'EST PAS UNE ARME
    /// </summary>
    public readonly GameObject AttackPrefab;

    /// <summary>
    /// L'effet qui sera appliqué au joueur si il consomme cet item. Ne sera jamais utilisé pour un non-consommable. Peut être null.
    /// </summary>
    public readonly Effect[] EffectUponConsumption;


    /// <summary>
    /// Le constructeur par défaut
    /// </summary>
    /// <param name="sprite">Le sprite relié à l'item</param>
    /// <param name="item">Les numériques de l'item</param>
    /// <param name="attackPrefab">Le préfab instancié à l'attaque. Nul par défaut</param>
    public ItemInInventory(Sprite sprite, Item item, GameObject attackPrefab = null, Effect[] effect = null)
    {
      Sprite = sprite;
      Item = item;
      AttackPrefab = attackPrefab;
      EffectUponConsumption = effect;
    }

    /// <summary>
    /// Renvoie une copie "deep" de l'instance. Copie aussi l'item.
    /// </summary>
    /// <returns>Le clone de l'instance</returns>
    public object Clone()
    {
      return new ItemInInventory(Sprite,(Item)Item.Clone(),AttackPrefab,EffectUponConsumption);
    }
  }

}
