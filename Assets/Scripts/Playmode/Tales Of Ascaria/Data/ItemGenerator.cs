using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Random = UnityEngine.Random;

namespace TalesOfAscaria
{
  /// <summary>
  /// Générateur d'item aléatoire. Utilisera une base, le niveau ainsi que la rareté afin de générer les stats.
  /// </summary>
  /// <remarks>
  /// A.Gagné
  /// </remarks>
  public sealed class ItemGenerator : GameScript
  {
    #region Editor

    [Header("Répértoire")]
#if UNITY_EDITOR
    [Help("Doit absolument être réglé, sinon le générateur d'item ne fera jamais rien", MessageType.Warning)]
#endif
    [Tooltip("Le répértoire de tous les items, préfixes et suffixes")] [Space(20)] [SerializeField]
    private ItemRepertoire repertoire;

    [Space(30)] [Header("Multiplicateurs")] [Space(20)]
    [Tooltip("La valeur par laquelle le générateur multipliera les stats des items commun")] [SerializeField]
    private float commonMultiplicator;

    [Tooltip("La valeur par laquelle le générateur multipliera les stats des items moins commun")] [SerializeField]
    private float uncommonMultiplicator;

    [Tooltip("La valeur par laquelle le générateur multipliera les stats des items rares")] [SerializeField]
    private float rareMultiplicator;

    [Tooltip("La valeur par laquelle le générateur multipliera les stats des items très rare")] [SerializeField]
    private float veryRareMultiplicator;

    [Tooltip("La valeur par laquelle le générateur multipliera les stats des items légendaires")] [SerializeField]
    private float legendaryMultiplicator;

    [Tooltip("La valeur par laquelle le générateur multipliera les stats des items mythiques")] [SerializeField]
    private float mythicMultiplicator;

    [Space(30)] [Header("Probabilités")] [Space(20)]
    [Help("Si les valeurs additionnées ne donnent pas 100%, elles seront automatiquement normalisées")]
    [Tooltip("Les probabilités de tomber sur un item commun")] [SerializeField] private float percentCommon;

    [Tooltip("Les probabilités de tomber sur un item moins commun")] [SerializeField] private float percentUncommon;
    [Tooltip("Les probabilités de tomber sur un item rare")] [SerializeField] private float percentRare;
    [Tooltip("Les probabilités de tomber sur un item très rare")] [SerializeField] private float percentVeryRare;
    [Tooltip("Les probabilités de tomber sur un item légendaire")] [SerializeField] private float percentLegendary;
    [Tooltip("Les probabilités de tomber sur un item mythique")] [SerializeField] private float percentMythic;

    [Space(30)] [Header("Icones et liens")] [Space(10)] [Tooltip("Les icones des items. Doit être dans l'ordre des ID")]
    [SerializeField] private Sprite[] allItemSprites;

    [Space(10)] [Tooltip("Les liens d'attaque des items. Doit être dans l'ordre des ID, peut être null")]
    [SerializeField] private List<GameObjectLink> allItemLinks;

    [Space(10)] [Tooltip(
      "Les effets de chaques item lorsque consummé. Les heals doivent être à 0, puisqu'ils seront réglés au runtime." +
      " Doit être dans l'ordre des ID. Peut être null")]
    [SerializeField] private List<ConsummableEffectLink> allItemEffects;

    #endregion

    #region PrivateAttributes

    private string[][] allPrefixes;
    private string[][] allSuffixes;
    private string[] mythicOverride;

    private ItemRepertoire.NonUsableData[] allNonUsableDatas;
    private ItemRepertoire.WeaponData[] allWeaponDatas;
    private ItemRepertoire.ArmorData[] allArmorDatas;
    private ItemRepertoire.ConsumableData[] allConsumableDatas;

    #endregion

    private void Awake()
    {
      if (repertoire == null)
      {
        Debug.LogError("Le répértoire est null: les items ne seront pas générés.");
        return;
      }
      ExtractFromRepertoire();
      CheckForIDDuplicates();
    }

    /// <summary>
    /// Retourne un NonUsable généré à partir du répértoire, en utilisant un ID et une rareté.
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="rarity">La rareté de l'item</param>
    /// <returns>Un NonUsable généré</returns>
    private NonUsable GetRandomNonUsable(int itemID, ItemRarity rarity)
    {
      ItemRepertoire.NonUsableData data = new ItemRepertoire.NonUsableData();
      bool itemFound = false;

      foreach (ItemRepertoire.NonUsableData nonUsableData in allNonUsableDatas)
      {
        if (nonUsableData.itemID == itemID)
        {
          data = nonUsableData;
          itemFound = true;
          break;
        }
      }
      if (!itemFound)
      {
        Debug.LogError("L'item ne se trouve pas dans le répértoire.");
        return null;
      }

      return new NonUsable(itemID, data.itemName, data.itemValue, rarity, data.description);
    }

    /// <summary>
    /// Retourne une arme générée aléatoirement selon les paramètres entrés
    /// </summary>
    /// <param name="itemID">Le ID de l'arme</param>
    /// <param name="targetLevel">Le niveau de l'arme (aussi le niveau requis pour la porter)</param>
    /// <param name="rarity">La rareté de l'arme</param>
    /// <returns>L'arme construite.</returns>
    private Weapon GetRandomWeapon(int itemID, int targetLevel, ItemRarity rarity)
    {
      ItemRepertoire.WeaponData data = new ItemRepertoire.WeaponData();
      bool itemFound = false;

      foreach (ItemRepertoire.WeaponData weaponData in allWeaponDatas)
      {
        if (weaponData.itemID == itemID)
        {
          data = weaponData;
          itemFound = true;
          break;
        }
      }

      if (!itemFound)
      {
        Debug.LogError("L'arme ne se trouve pas dans le répértoire.");
        return null;
      }
      float levelMultiplier = GetMultiplierForStatsWithLevel(targetLevel);
      float rarityMultiplier = RarityToMultiplier(rarity);

      float globalMultiplier = levelMultiplier * rarityMultiplier * GetRandomMultiplier();
      float cooldownMultiplier = Mathf.Max(1f, Mathf.Pow(rarityMultiplier, 0.3f));

      string generatedName = "";

      if (rarity == ItemRarity.Mythic)
      {
        generatedName = GetRandomPrefix(rarity) + " " + GetRandomMythicName(itemID) + " " + GetRandomSuffix(rarity);
      }
      else
      {
        generatedName = GetRandomPrefix(rarity) + " " + data.weaponName + " " + GetRandomSuffix(rarity);
      }

      return new Weapon(itemID, generatedName, Mathf.RoundToInt(data.weaponValue * globalMultiplier), rarity,
        targetLevel,
        Mathf.RoundToInt(data.weaponDamage * globalMultiplier),
        RoundToTwoDigits(data.weaponAttacksPerSecond * cooldownMultiplier),
        RoundToTwoDigits(data.weaponSecondaryAttackCooldown / cooldownMultiplier), data.weaponWisdomMultiplier,
        data.weaponStrengthMultiplier,
        Mathf.RoundToInt(data.bonusWisdom * globalMultiplier), Mathf.RoundToInt(data.bonusStrength * globalMultiplier));
    }

    /// <summary>
    /// Retourne une arme générée aléatoirement selon les paramètre entrés.
    /// La rareté sera aléatoire, selon les possibilités réglés dans l'éditeur.
    /// </summary>
    /// <param name="itemID">Le ID de l'arme</param>
    /// <param name="targetLevel">Le niveau de l'arme (aussi le niveau requis pour la porter)</param>
    /// <returns>L'arme générée.</returns>
    private Weapon GetRandomWeapon(int itemID, int targetLevel)
    {
      return GetRandomWeapon(itemID, targetLevel, GetRandomRarity());
    }


    /// <summary>
    /// Retourne une armure générée aléatoirement selon les paramètres entrés
    /// </summary>
    /// <param name="itemID">Le ID de l'armure</param>
    /// <param name="targetLevel">Le niveau de l'armure (aussi le niveau requis pour la porter)</param>
    /// <param name="rarity">La rareté de l'armure</param>
    /// <returns>L'armure construite.</returns>
    private Armor GetRandomArmor(int itemID, int targetLevel, ItemRarity rarity)
    {
      ItemRepertoire.ArmorData data = new ItemRepertoire.ArmorData();
      bool itemFound = false;

      foreach (ItemRepertoire.ArmorData armorData in allArmorDatas)
      {
        if (armorData.itemID == itemID)
        {
          data = armorData;
          itemFound = true;
          break;
        }
      }

      if (!itemFound)
      {
        Debug.LogError("L'armure ne se trouve pas dans le répértoire.");
        return null;
      }
      float levelMultiplier = GetMultiplierForStatsWithLevel(targetLevel);
      float rarityMultiplier = RarityToMultiplier(rarity);

      float globalMultiplier = levelMultiplier * rarityMultiplier * GetRandomMultiplier();


      string generatedName = "";

      if (rarity == ItemRarity.Mythic)
      {
        generatedName = GetRandomPrefix(rarity) + " " + GetRandomMythicName(itemID) + " " + GetRandomSuffix(rarity);
      }
      else
      {
        generatedName = GetRandomPrefix(rarity) + " " + data.armorName + " " + GetRandomSuffix(rarity);
      }

      return new Armor(itemID, generatedName, Mathf.RoundToInt(data.armorValue * globalMultiplier), rarity, targetLevel,
        Mathf.RoundToInt(data.bonusHealth * globalMultiplier),
        Mathf.RoundToInt(data.bonusMana * globalMultiplier),
        Mathf.RoundToInt(data.bonusConstitution * globalMultiplier),
        Mathf.RoundToInt(data.bonusSpirit * globalMultiplier),
        Mathf.RoundToInt(data.bonusWisdom * globalMultiplier),
        Mathf.RoundToInt(data.bonusStrength * globalMultiplier));
    }

    /// <summary>
    /// Retourne une armure générée aléatoirement selon les paramètres entrés.
    /// La rareté sera aléatoire, selon les possibilités réglés dans l'éditeur.
    /// </summary>
    /// <param name="itemID">Le ID de l'armure</param>
    /// <param name="targetLevel">Le niveau de l'armure (aussi le niveau requis pour la porter)</param>
    /// <param name="rarity">La rareté de l'armure</param>
    /// <returns>L'armure construite.</returns>
    private Armor GetRandomArmor(int itemID, int targetLevel)
    {
      return GetRandomArmor(itemID, targetLevel, GetRandomRarity());
    }

    /// <summary>
    /// Retourne un consumable généré aléatoirement
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="targetLevel">Le niveau de l'item</param>
    /// <param name="rarity">la rareté de l'item</param>
    /// <returns>Le consomable</returns>
    private Consumable GetRandomConsumable(int itemID, int targetLevel, ItemRarity rarity)
    {
      ItemRepertoire.ConsumableData data = new ItemRepertoire.ConsumableData();
      bool itemFound = false;

      foreach (ItemRepertoire.ConsumableData consumableData in allConsumableDatas)
      {
        if (consumableData.itemID == itemID)
        {
          data = consumableData;
          itemFound = true;
          break;
        }
      }
      if (!itemFound)
      {
        Debug.LogError("L'item ne se trouve pas dans le répértoire.");
        return null;
      }
      float power = 0;
      if (data.multiplyEffect)
      {
        power = RoundToTwoDigits(data.power * GetMultiplierForStatsWithLevel(targetLevel) *
                                 RarityToMultiplier(rarity) * GetRandomMultiplier());
      }
      else
      {
        power = data.power;
      }
      int value = Mathf.RoundToInt(data.consumableValue * GetMultiplierForStatsWithLevel(targetLevel) *
                                   RarityToMultiplier(rarity) * GetRandomMultiplier());
      string generatedName = "";

      if (rarity == ItemRarity.Mythic)
      {
        generatedName = GetRandomPrefix(rarity) + " " + GetRandomMythicName(itemID) + " " + GetRandomSuffix(rarity);
      }
      else
      {
        generatedName = GetRandomPrefix(rarity) + " " + data.consumableName + " " + GetRandomSuffix(rarity);
      }

      return new Consumable(itemID, generatedName, value, rarity, power, data.description);
    }

    /// <summary>
    /// Retourne un consumable généré aléatoirement.
    /// La rareté sera aléatoire, selon les possibilités réglés dans l'éditeur.
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="targetLevel">Le niveau de l'item</param>
    /// <param name="rarity">la rareté de l'item</param>
    /// <returns>Le consomable</returns>
    private Consumable GetRandomConsumable(int itemID, int targetLevel)
    {
      return GetRandomConsumable(itemID, targetLevel, GetRandomRarity());
    }

    /// <summary>
    /// Retourne un item généré aléatoirement selon les paramètre donnés.
    /// </summary>
    /// <param name="itemID">Le ID de l'item.</param>
    /// <param name="rarity">La rareté de l'item</param>
    /// <param name="targetLevel">Le niveau de l'item</param>
    /// <returns>L'item généré</returns>
    public Item GetRandomItem(int itemID, ItemRarity rarity, int targetLevel = 1)
    {
      foreach (ItemRepertoire.NonUsableData nonUsableData in allNonUsableDatas)
      {
        if (nonUsableData.itemID == itemID)
        {
          return GetRandomNonUsable(itemID, rarity);
        }
      }
      foreach (ItemRepertoire.WeaponData weaponData in allWeaponDatas)
      {
        if (weaponData.itemID == itemID)
        {
          return GetRandomWeapon(itemID, targetLevel, rarity);
        }
      }
      foreach (ItemRepertoire.ArmorData armorData in allArmorDatas)
      {
        if (armorData.itemID == itemID)
        {
          return GetRandomArmor(itemID, targetLevel, rarity);
        }
      }
      foreach (ItemRepertoire.ConsumableData consumableData in allConsumableDatas)
      {
        if (consumableData.itemID == itemID)
        {
          return GetRandomConsumable(itemID, targetLevel, rarity);
        }
      }
      Debug.LogError("Une erreur inatendue s'est produite");
      return null;
    }

    /// <summary>
    /// Retourne un item généré aléatoirement selon les paramètre donnés. La rareté est aléatoire, selon les
    /// paramètre réglés dans l'éditeur.
    /// </summary>
    /// <param name="itemID">Le ID de l'item.</param>
    /// <param name="targetLevel">Le niveau de l'item</param>
    /// <returns>L'item généré</returns>
    public Item GetRandomItem(int itemID, int targetlevel = 1)
    {
      return GetRandomItem(itemID, GetRandomRarity(), targetlevel);
    }

    /// <summary>
    /// Retourne le préfab de l'item pour attaquer, peut être null.
    /// </summary>
    /// <param name="itemID">Le ID</param>
    /// <returns>Le préfab</returns>
    public GameObject GetItemLink(int itemID)
    {
      foreach (GameObjectLink gameObjectLink in allItemLinks)
      {
        if (gameObjectLink.itemID == itemID)
        {
          return gameObjectLink.gameobjectLink;
        }
      }
      return null;
    }

    /// <summary>
    /// Retourne l'image de l'item.
    /// </summary>
    /// <param name="itemID">Le ID</param>
    /// <returns>L'icone de l'item</returns>
    public Sprite GetItemSprite(int itemID)
    {
      if (allItemSprites.Length <= itemID)
      {
        Debug.LogError("Le tableau est trop petit. Svp ajuster le tableau du generateur");
        return null;
      }
      return allItemSprites[itemID];
    }

    /// <summary>
    /// Retourne l'effet de l'item.
    /// </summary>
    /// <param name="itemID">Le ID</param>
    /// <returns>L'effet de l'item</returns>
    public Effect[] GetItemEffect(int itemID)
    {
      ConsumableEffects effect = null;
      foreach (ConsummableEffectLink consummableEffectLink in allItemEffects)
      {
        if (consummableEffectLink.itemID == itemID)
        {
          effect = consummableEffectLink.consumableEffects;
          break;
        }
      }
      if (effect != null)
      {
        return effect.effects;
      }
      return null;
    }

    /// <summary>
    /// Retourne le ID maximum d'un item. Retourne -1 si aucun item n'existe.
    /// </summary>
    /// <returns>Le ID maximum des items</returns>
    public int GetMaximumItemID()
    {
      if (allItemSprites == null || allItemSprites.Length == 0)
      {
        return -1;
      }
      return allItemSprites.Length - 1;
    }
    
    #region PrivateMethods

    /// <summary>
    /// Retourne un chiffre float entre 0.7 et 1.3.
    /// </summary>
    /// <returns>Un chiffre entre 0.7 et 1.3</returns>
    private float GetRandomMultiplier()
    {
      return Random.Range(0.7f, 1.3f);
    }

    /// <summary>
    /// Retourne une rareté selon les chances spécifié dans l'inspecteur.
    /// </summary>
    /// <returns>La rareté générée</returns>
    private ItemRarity GetRandomRarity()
    {
      float maximumNumber = percentCommon + percentLegendary + percentMythic + percentRare + percentUncommon +
                            percentVeryRare;
      if (maximumNumber != 100f)
      {
        Debug.Log("Le total des pourcentage n'est pas 100. Le sytème le supporte, mais ce n'est pas très intuitif...");
      }
      float randomNumber = Random.Range(0f, maximumNumber);
      float accumulatedNumber = percentCommon;

      if (randomNumber <= accumulatedNumber)
      {
        return ItemRarity.Common;
      }
      accumulatedNumber += percentUncommon;
      if (randomNumber <= accumulatedNumber)
      {
        return ItemRarity.Uncommon;
      }
      accumulatedNumber += percentRare;
      if (randomNumber <= accumulatedNumber)
      {
        return ItemRarity.Rare;
      }
      accumulatedNumber += percentVeryRare;
      if (randomNumber <= accumulatedNumber)
      {
        return ItemRarity.Very_Rare;
      }
      accumulatedNumber += percentLegendary;
      if (randomNumber <= accumulatedNumber)
      {
        return ItemRarity.Legendary;
      }
      return ItemRarity.Mythic;
    }


    /// <summary>
    /// Copie toutes les informations du répértoire pour un accès plus rapide et plus propre.
    /// </summary>
    private void ExtractFromRepertoire()
    {
      allWeaponDatas = repertoire.weapons.ToArray();
      allArmorDatas = repertoire.armors.ToArray();
      allNonUsableDatas = repertoire.nonUsableItems.ToArray();
      allConsumableDatas = repertoire.consumables.ToArray();

      mythicOverride = repertoire.mythicItemNameOverride;

      allPrefixes = new string[6][];
      allSuffixes = new string[6][];

      allPrefixes[0] = repertoire.commonItemPrefix.ToArray();
      allPrefixes[1] = repertoire.uncommonItemPrefix.ToArray();
      allPrefixes[2] = repertoire.rareItemPrefix.ToArray();
      allPrefixes[3] = repertoire.veryRareItemPrefix.ToArray();
      allPrefixes[4] = repertoire.LegendaryItemPrefix.ToArray();
      allPrefixes[5] = repertoire.mythicItemPrefix.ToArray();

      allSuffixes[0] = repertoire.commonItemSuffix.ToArray();
      allSuffixes[1] = repertoire.uncommonItemSuffix.ToArray();
      allSuffixes[2] = repertoire.rareItemSuffix.ToArray();
      allSuffixes[3] = repertoire.veryRareItemSuffix.ToArray();
      allSuffixes[4] = repertoire.LegendaryItemSuffix.ToArray();
      allSuffixes[5] = repertoire.mythicItemSuffix.ToArray();
    }

    /// <summary>
    /// Retourne le multiplicateur basé sur la rareté
    /// </summary>
    /// <param name="rarity">La rareté</param>
    /// <returns>Le multiplicateur</returns>
    private float RarityToMultiplier(ItemRarity rarity)
    {
      switch (rarity)
      {
        case ItemRarity.Common:
          return commonMultiplicator;
        case ItemRarity.Uncommon:
          return uncommonMultiplicator;
        case ItemRarity.Rare:
          return rareMultiplicator;
        case ItemRarity.Very_Rare:
          return veryRareMultiplicator;
        case ItemRarity.Legendary:
          return legendaryMultiplicator;
        case ItemRarity.Mythic:
          return mythicMultiplicator;
      }
      return 1f;
    }

    /// <summary>
    /// Retourne un préfixe au hasard correspondant à la rareté donnée
    /// </summary>
    /// <param name="rarity">La rareté de l'item</param>
    /// <returns>Le préfixe</returns>
    private string GetRandomPrefix(ItemRarity rarity)
    {
      int rarityNumber = (int) rarity;
      int randomNumber = Random.Range(0, allPrefixes[rarityNumber].Length);
      return allPrefixes[rarityNumber][randomNumber];
    }

    /// <summary>
    /// Retourne un suffixe au hasard correspondant à la rareté donnée
    /// </summary>
    /// <param name="rarity">La rareté de l'item</param>
    /// <returns>Le suffixe</returns>
    private string GetRandomSuffix(ItemRarity rarity)
    {
      int rarityNumber = (int) rarity;
      int randomNumber = Random.Range(0, allSuffixes[rarityNumber].Length);
      return allSuffixes[rarityNumber][randomNumber];
    }

    /// <summary>
    /// Arrondi pour donner un chiffre X.xx
    /// </summary>
    /// <param name="number">Le nombre</param>
    /// <returns>le nombre arrondi</returns>
    public static float RoundToTwoDigits(float number)
    {
      float returnValue = number;
      returnValue *= 100;
      returnValue = Mathf.RoundToInt(returnValue);
      return (returnValue / 100f);
    }

    /// <summary>
    /// Retourne un nom aléatoire pour un item mythique
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <returns>Le nom de l'item</returns>
    private string GetRandomMythicName(int itemID)
    {
      return mythicOverride[itemID];
    }

    /// <summary>
    /// Retourne le multiplicateur des stats pour un item de niveau X.
    /// </summary>
    /// <param name="level">Le niveau de l'item</param>
    /// <returns>Le multiplicateur à appliquer</returns>
    public static float GetMultiplierForStatsWithLevel(int level)
    {
      return (1f + ((level - 1) * 0.5f));
    }

    /// <summary>
    /// Vérifie qu'il n'y ait aucun doublons dans les ItemIDs
    /// </summary>
    private void CheckForIDDuplicates()
    {
      bool[] idChecks = new bool[allConsumableDatas.Length + allArmorDatas.Length + allWeaponDatas.Length +
                                 allNonUsableDatas.Length];
      bool hasError = false;

      foreach (ItemRepertoire.ConsumableData consumableData in allConsumableDatas)
      {
        if (idChecks[consumableData.itemID])
        {
          Debug.LogError("There is a duplicate with the ID " + consumableData.itemID +
                         ". Expect weird behavior from the item generator");
          hasError = true;
        }
        idChecks[consumableData.itemID] = true;
      }
      foreach (ItemRepertoire.WeaponData weaponData in allWeaponDatas)
      {
        if (idChecks[weaponData.itemID])
        {
          Debug.LogError("There is a duplicate with the ID " + weaponData.itemID +
                         ". Expect weird behavior from the item generator");
          hasError = true;
        }
        idChecks[weaponData.itemID] = true;
      }
      foreach (ItemRepertoire.ArmorData armorData in allArmorDatas)
      {
        if (idChecks[armorData.itemID])
        {
          Debug.LogError("There is a duplicate with the ID " + armorData.itemID +
                         ". Expect weird behavior from the item generator");
          hasError = true;
        }
        idChecks[armorData.itemID] = true;
      }
      foreach (ItemRepertoire.NonUsableData nonUsableData in allNonUsableDatas)
      {
        if (idChecks[nonUsableData.itemID])
        {
          Debug.LogError("There is a duplicate with the ID " + nonUsableData.itemID +
                         ". Expect weird behavior from the item generator");
          hasError = true;
        }
        idChecks[nonUsableData.itemID] = true;
      }

      if (hasError)
      {
        Debug.Break();
      }
    }

    #endregion

  }


  /// <summary>
  /// Représente un lien entre un objet et un gameobject
  /// </summary>
  [Serializable]
  public struct GameObjectLink
  {
    [Tooltip("Le ID de l'arme.")]
    public int itemID;
    [Tooltip("Le GameObject qui sera spawné pour une attaque primaire.")]
    public GameObject gameobjectLink;
  }


  /// <summary>
  /// Représente un lien entre un objet et un effectContainer
  /// </summary>
  [Serializable]
  public struct ConsummableEffectLink
  {
    [Tooltip("Le ID du consommable.")]
    public int itemID;
    [Tooltip("L'effect qui sera utilisé lorsque l'objet sera consumé")]
    public ConsumableEffects consumableEffects;
  }
}