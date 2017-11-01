using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TalesOfAscaria
{

  /// <summary>
  /// Le répértoire de tous les stats des items
  /// </summary>
  /// <remarks>
  /// A.Gagné
  /// </remarks>
  [CreateAssetMenu(fileName = "ItemRepertoire", menuName = "Game/Items/Repertoire")]
  /*
   * Note: Il peut arriver que cet objet soit vu comme null. Si c'est le cas, il faudra ouvrir le .meta et
   * re-régler le guid du script (qui est corrompu) au bon guid du script "ItemRepertoire"
  */
  public sealed class ItemRepertoire : ScriptableObject
  {

    [Space(20)]
    [Header("Items")]
    [Space(10)]

    [Header("Non-Utilisables")]
    [Space(10)]
    [Tooltip("La liste de tous les items non-utilisable")]
    public List<NonUsableData> nonUsableItems;

    [Space(15)]
    [Header("Armes")]
    [Space(10)]
    [Tooltip("La liste de toutes les armes. (Équivaut aux stats d'une arme de rareté commune, de niveau 1)")]
    public List<WeaponData> weapons;

    [Space(15)]
    [Header("Armures")]
    [Space(10)]
    [Tooltip("La liste de toutes les armures. (Équivaut aux stats d'une armure de rareté commune, de niveau 1")]
    public List<ArmorData> armors;

    [Space(15)]
    [Header("Consommables")]
    [Space(10)]
    [Tooltip("La liste de toutes les consumables.")]
    public List<ConsumableData> consumables;

    [Space(40)]
    [Header("Prefixes d'item")]
    [Space(10)]
    [Tooltip("Les préfixes possibles d'items communs")]
    public List<string> commonItemPrefix;
    [Tooltip("Les préfixes possibles d'items moins communs")]
    public List<string> uncommonItemPrefix;
    [Tooltip("Les préfixes possibles d'items rares")]
    public List<string> rareItemPrefix;
    [Tooltip("Les préfixes possibles d'items très rare")]
    public List<string> veryRareItemPrefix;
    [Tooltip("Les préfixes possibles d'items legendaires")]
    public List<string> LegendaryItemPrefix;
    [Tooltip("Les préfixes possibles d'items mythiques")]
    public List<string> mythicItemPrefix;

    [Space(20)]
    [Header("Suffixes d'item")]
    [Space(10)]
    [Tooltip("Les suffixes possibles d'items communs")]
    public List<string> commonItemSuffix;
    [Tooltip("Les suffixes possibles d'items moins communs")]
    public List<string> uncommonItemSuffix;
    [Tooltip("Les suffixes possibles d'items rares")]
    public List<string> rareItemSuffix;
    [Tooltip("Les suffixes possibles d'items très rare")]
    public List<string> veryRareItemSuffix;
    [Tooltip("Les suffixes possibles d'items legendaires")]
    public List<string> LegendaryItemSuffix;
    [Tooltip("Les suffixes possibles d'items mythiques")]
    public List<string> mythicItemSuffix;

    [Space(10)]
    [Header("Override de noms pour les mythiques")]
    [Space(10)]
    [Tooltip("Le nom pour chaque itemID, qui remplacera le nom d'item si celui-ci est mythique")]
    public string[] mythicItemNameOverride;


    [Serializable]
    public struct NonUsableData
    {
      [Tooltip("Le ID de l'item. Doit être unique")]
      public int itemID;
      [Tooltip("Le nom de l'item, sans aucun préfixe ou suffixes")]
      public string itemName;
      [Tooltip("La valeur de l'item")]
      public int itemValue;

      [Tooltip("La description affichée au joueur")]
      public string description;
    }

    [Serializable]
    public struct WeaponData
    {
      [Tooltip("Le ID de l'item. Doit être unique")]
      public int itemID;
      [Tooltip("Le nom de l'arme, sans aucun préfixe ou suffixes")]
      public string weaponName;
      [Tooltip("La valeur de l'arme")]
      public int weaponValue;

      [Tooltip("Le nombre de wisdom que l'arme donnera")]
      public int bonusWisdom;
      [Tooltip("Le nombre de strength que l'arme donnera")]
      public int bonusStrength;

      [Tooltip("Le dégât par attaque")]
      public int weaponDamage;
      [Tooltip("Le nombre d'attaques par " +
               "secondes pouvant être éfectuées")]
      public float weaponAttacksPerSecond;
      [Tooltip("Le cooldown de l'action secondaire")]
      public float weaponSecondaryAttackCooldown;
      [Tooltip("Le multiplicateur de wisdom sur le dégât de l'arme")]
      public float weaponWisdomMultiplier;
      [Tooltip("Le multiplicateur de strength sur le dégât de l'arme")]
      public float weaponStrengthMultiplier;
    }

    [Serializable]
    public struct ArmorData
    {
      [Tooltip("Le ID de l'item. Doit être unique")]
      public int itemID;
      [Tooltip("Le nom de l'armure, sans aucun préfixe ou suffixes")]
      public string armorName;
      [Tooltip("La valeur de l'armure")]
      public int armorValue;

      [Tooltip("Le nombre de wisdom que l'armure donnera")]
      public int bonusWisdom;
      [Tooltip("Le nombre de strength que l'armure donnera")]
      public int bonusStrength;

      [Tooltip("Le nombre de vie que l'armure donnera")]
      public int bonusHealth;
      [Tooltip("Le nombre de mana que l'armure donnera")]
      public int bonusMana;
      [Tooltip("Le nombre de constitution que l'armure donnera")]
      public int bonusConstitution;
      [Tooltip("Le nombre de spirit que l'armure donnera")]
      public int bonusSpirit;

    }

    [Serializable]
    public struct ConsumableData
    {
      [Tooltip("Le ID de l'item. Doit être unique")]
      public int itemID;
      [Tooltip("Le nom du consumable")]
      public string consumableName;
      [Tooltip("La valeur du consumable")]
      public int consumableValue;

      [Tooltip("La description affichée au joueur")]
      public string description;
      [Tooltip("La puissance globale du consomable au niveau 1. Affectera tous ses effets")]
      public float power;
      [Tooltip("L'effet sera-t-il multiplié?")]
      public bool multiplyEffect;
    }
  }
}


