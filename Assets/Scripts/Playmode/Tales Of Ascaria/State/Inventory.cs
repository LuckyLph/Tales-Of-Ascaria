using System;
using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TalesOfAscaria
{
  /// <summary>
  /// Cette classe représente l'inventaire d'un des joueur ainsi que son capital.
  /// </summary>
  /// <remarks>
  /// Via cette classe, il est possible d'ajouter n'importe quel item, d'en retirer et faire des vérifications.
  /// On peut aussi gérer l'économie
  /// 
  /// A.Gagné
  /// </remarks>
  public sealed class Inventory : GameScript
  {

    /// <summary>
    /// Lorsqu'un item est enlevé ou ajouté
    /// </summary>
    /// <param name="item">L'item en question</param>
    public delegate void ItemMovedHandler(ItemInInventory item);
    /// <summary>
    /// Lorsqu'une pièce d'équipement est changée
    /// </summary>
    /// <param name="item">Le nouvel équipement. Peut être nul</param>
    public delegate void EquipementEquipedHandler(ItemInInventory item);
    /// <summary>
    /// Lorsque l'argent du joueur est modifié
    /// </summary>
    /// <param name="moneyNow">L'argent après la modification</param>
    public delegate void EconomyEventHandler(uint moneyNow);

    /// <summary>
    /// Appellé lorsqu'un item est ajouté à l'inventaire
    /// </summary>
    public event ItemMovedHandler OnItemAdded;
    /// <summary>
    /// Appellé lorsqu'un item est supprimé de l'inventaire
    /// </summary>
    public event ItemMovedHandler OnItemRemoved;
    /// <summary>
    /// Appellé lorsque l'arme du joueur a été modifiée. Peut être null
    /// </summary>
    public event EquipementEquipedHandler OnWeaponChanged;
    /// <summary>
    /// Appellé lorsque l'armure du joueur a été modifiée. Peut être null
    /// </summary>
    public event EquipementEquipedHandler OnArmorChanged;
    /// <summary>
    /// Appellé lorsque l'argent du joueur a été modifiée.
    /// </summary>
    public event EconomyEventHandler OnMoneyChanged;
    /// <summary>
    /// L'inventaire du joueur (tous ses items)
    /// </summary>
    private List<ItemInInventory> inventory;
    /// <summary>
    /// Le pointeur vers l'arme actuelle du joueur. Se trouve aussi dans l'inventaire. Peut être null
    /// </summary>
    private ItemInInventory currentWeapon;
    /// <summary>
    /// Le pointeur vers l'armure actuelle du joueur. Se trouve aussi dans l'inventaire. Peut être null
    /// </summary>
    private ItemInInventory currentArmor;

    private ItemAdder itemAdder;

    /// <summary>
    /// L'argent actuel du joueur
    /// </summary>
    public uint Money
    {
      get;
      private set;
    }

    public uint PercentageMoneyLostOnDeath
    {
      get
      {
        return percentageMoneyLostOnDeath;
      }
      private set
      {
        percentageMoneyLostOnDeath = value;
      }
    }

    /// <summary>
    /// Le nombre maximum d'item que le joueur peut porter
    /// </summary>
    [Range(1f,InventoryView.MaximumItemsDisplayed)]
    [Tooltip("Le nombre maximum d'item que le joueur peut posséder")]
    [SerializeField] private int maximumItemsInInventory;

    /// <summary>
    /// Le pourcentage d'argent perdu à la mort
    /// </summary>
    [Tooltip("Le pourcentage d'argent perdu à la mort")]
    [SerializeField] private uint percentageMoneyLostOnDeath;

    //Liens

    private LivingEntity livingEntity;

    

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
      InjectDependencies("InjectInventory");
      inventory = new List<ItemInInventory>();

      OnArmorChanged += CheckIfEquiped;
      OnWeaponChanged += CheckIfEquiped;
    }

    private void OnDestroy()
    {
      OnArmorChanged -= CheckIfEquiped;
      OnWeaponChanged -= CheckIfEquiped;
    }

    private void InjectInventory([EntityScope] LivingEntity livingEntity,[EntityScope] ItemAdder itemAdder)
    {
      this.livingEntity = livingEntity;
      this.itemAdder = itemAdder;
    }

    #region Informations
    /// <summary>
    /// Retourne la limite d'item de l'inventaire.
    /// </summary>
    /// <returns>La limite</returns>
    public int GetMaximumItems()
    {
      return maximumItemsInInventory;
    }
    /// <summary>
    /// Retourne le nombre de l'item actuellement dans l'inventaire
    /// </summary>
    /// <returns>Le nombre d'item dans l'inventaire actuellement</returns>
    public int GetNumberOfItems()
    {
      return inventory.Count;
    }
    /// <summary>
    /// Retourne tous les items dans un tableau
    /// </summary>
    /// <returns>Tous les items</returns>
    public ItemInInventory[] GetAllItems()
    {
      return inventory.ToArray();
    }
    /// <summary>
    /// Retourne tous les items d'un certain type
    /// </summary>
    /// <param name="itemType">Le type d'item à renvoyer</param>
    /// <returns>Les items dans un tableau</returns>
    public ItemInInventory[] GetAllItemsOfType(Type itemType)
    {
      List<ItemInInventory> liste = new List<ItemInInventory>();
      ItemInInventory[] allItems = GetAllItems();
      for (int i = 0; i < allItems.Length; i++)
      {
        if (allItems[i].Item.GetType() == itemType)
        {
         liste.Add(allItems[i]); 
        }
      }
      return liste.ToArray();
    }
    /// <summary>
    /// Retourne le nombre d'item correspondant à un itemID
    /// </summary>
    /// <param name="itemID">Le ID à vérifier</param>
    /// <returns>Le nombre d'item correspondant</returns>
    public int GetItemAmount(int itemID)
    {
      int amount = 0;
      ItemInInventory[] items = inventory.ToArray();
      for (int i = 0; i < items.Length; i++)
      {
        if (items[i].Item.ItemID == itemID)
        {
          amount++;
        }
      }
      return amount;
    }
    /// <summary>
    /// Retourne le nombre d'item d'un certain type
    /// </summary>
    /// <param name="itemType">Le type d'item</param>
    /// <returns>Le nombre d'item du type</returns>
    public int GetItemAmount(Type itemType)
    {
      int amount = 0;
      ItemInInventory[] items = inventory.ToArray();
      for (int i = 0; i < items.Length; i++)
      {
        if (items[i].Item.GetType() == itemType)
        {
          amount++;
        }
      }
      return amount;
    }
    /// <summary>
    /// Retourne si l'itemID est présent quelque part dans l'inventaire
    /// </summary>
    /// <param name="itemID">Le ID à vérifier</param>
    /// <returns>Si l'item existe</returns>
    public bool HasItem(int itemID)
    {
      ItemInInventory[] items = inventory.ToArray();
      for (int i = 0; i < items.Length; i++)
      {
        if (items[i].Item.ItemID == itemID)
        {
          return true;
        }
      }
      return false;
    }
    /// <summary>
    /// Retourne si l'item est présent quelque part dans l'inventaire
    /// </summary>
    /// <param name="item">L'item à vérifier</param>
    /// <returns>Si l'item existe</returns>
    public bool HasItem(ItemInInventory item)
    {
      ItemInInventory[] items = inventory.ToArray();
      for (int i = 0; i < items.Length; i++)
      {
        if (items[i] == item)
        {
          return true;
        }
      }
      return false;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Debug.Log le statut de l'inventaire. Utile pour débugger.
    /// </summary>
    [Obsolete("Ne fonctionne que dans l'éditeur",false)]
    public void LogAllItemIntoUnity()
    {
      ItemInInventory[] allItems = GetAllItems();

      Debug.Log("Debut de l'inventaire");

      foreach (ItemInInventory item in allItems)
      {
        LogItem(item);
      }

      Debug.Log("Argent: " + Money + "$");
      Debug.Log("Inventaire à " + ((float)GetNumberOfItems() / maximumItemsInInventory) * 100f + "% de sa capacité (" 
        + GetNumberOfItems() + "/" + maximumItemsInInventory + ")");
      Debug.Log("Fin de l'inventaire");
    }

    /// <summary>
    /// Décrit un item dans la console. Affichera tous ses stats
    /// </summary>
    /// <param name="item">L'item à décrire</param>
    private void LogItem(ItemInInventory item)
    {
      if (item.Item is NonUsable)
      {
        Debug.Log("- (ID: " + item.Item.ItemID + ") " + item.Item.Name + " [" + item.Item.Rarity.ToString().Replace("_", " ") + "]"
                  + ", vaut " + item.Item.Value + "$ -");
      }
      else if (item.Item is Weapon)
      {
        Weapon weapon = (Weapon) item.Item;;
        Debug.Log("- (ID: " + item.Item.ItemID + ") " + item.Item.Name + " [" + item.Item.Rarity.ToString().Replace("_", " ") + "]"
                  + ", vaut " + item.Item.Value + "$ -\n" + "Degats: " + weapon.BaseDamage + "\n" +
                  "Attack speed: " + weapon.AttackPerSecond + "\n" +
                  "Secondary action cooldown: " + weapon.SecondaryCooldown + "\n" +
                  "Scaling: " + weapon.StrengthMultiplier * 100f + "% STR, " + weapon.WisdomMultiplier * 100f + "% WIS\n" +
                  "Strength+: " + weapon.BonusStrength + "\n" +
                  "Wisdom+: " + weapon.BonusWisdom + "\n" +
                  "Required Level: " + weapon.RequiredLevel);
      }
      else if (item.Item is Armor)
      {
        Armor armor = (Armor)item.Item; ;
        Debug.Log("- (ID: " + item.Item.ItemID + ") " + item.Item.Name + " [" + item.Item.Rarity.ToString().Replace("_", " ") + "]"
                  + ", vaut " + item.Item.Value + "$ -\n" +
                  "HP+: " + armor.BonusHealth + "\n" +
                  "Mana+: " + armor.BonusMana + "\n" +
                  "Constitution+: " + armor.BonusConstitution + "\n" +
                  "Spirit+: " + armor.BonusSpirit + "\n" +
                  "Strength+: " + armor.BonusStrength + "\n" +
                  "Wisdom+: " + armor.BonusWisdom + "\n" +
                  "Required Level: " + armor.RequiredLevel);
      }

      else if (item.Item is Consumable)
      {
        Consumable consumable = (Consumable) item.Item;
        string formatedDescription = String.Format(consumable.Description, Mathf.RoundToInt(consumable.Power));
        string value = (consumable.Name
                        + "\nConsumable"
                        + "\nWorth: " + consumable.Value + "$"
                        + "\n\n" + formatedDescription);
        Debug.Log(value);
      }
    }
#endif
    #endregion

    #region Economy

    /// <summary>
    /// Règle manuellement l'argent. Envoie un avertissement (non souhaité)
    /// </summary>
    /// <param name="money">L'argent du joueur</param>
    public void SetMoney(uint money)
    {
      Debug.Log("L'argent a été modifiée manuellement. Est-ce voulu?");
      Money = (uint)Mathf.Clamp(money,uint.MinValue,uint.MaxValue);
      if (OnMoneyChanged != null) OnMoneyChanged(Money);
    }
    /// <summary>
    /// Ajoute de l'argent au joueur
    /// </summary>
    /// <param name="amount">Le montant</param>
    public void AddMoney(uint amount)
    {
      Money = (uint)Mathf.Min(Money + amount, uint.MaxValue);
      if (OnMoneyChanged != null) OnMoneyChanged(Money);
    }
    /// <summary>
    /// Retire de l'argent au joueur
    /// </summary>
    /// <param name="amount">Le montant</param>
    public void RemoveMoney(uint amount)
    {
      Money = (uint) Mathf.Max(Money - amount, uint.MinValue);
      if (OnMoneyChanged != null) OnMoneyChanged(Money);
    }
    /// <summary>
    /// Tente d'acheter un item. Retourne si l'achat fut réussi
    /// </summary>
    /// <param name="price">Le prix de l'achat</param>
    /// <returns>L'achat a-t-il été complété?</returns>
    public bool Buy(uint price)
    {
      if (Money >= price)
      {
        Money -= price;
        if (OnMoneyChanged != null) OnMoneyChanged(Money);
        return true;
      }
      return false;
    }
    /// <summary>
    /// Vérifie si le joueur possède au moins un certain montant d'argent
    /// </summary>
    /// <param name="amount">Le montant</param>
    /// <returns>Si le joueur possède cet argent</returns>
    public bool HasAtLeast(uint amount)
    {
      return Money >= amount;
    }
    /// <summary>
    /// Remet l'argent à 0. Lance un avertissement (non souhaité)
    /// </summary>
    public void ClearMoney()
    {
      Debug.Log("L'argent a été remis à 0. Est-ce voulu?");
      Money = 0;
      if (OnMoneyChanged != null) OnMoneyChanged(Money);
    }

    #endregion

    #region ItemManagers


    

    /// <summary>
    /// Ajoute un item préfait.
    /// </summary>
    /// <param name="item">L'item, incluant son icone et son lien au préfab</param>
    /// <returns>Si l'item a bel et bien été ajouté</returns>
    public bool AddItem(ItemInInventory item)
    {
      if (item == null)
      {
        Debug.LogError("L'item est null");
        return false;
      }
      if (inventory.Count >= maximumItemsInInventory)
      {
        Debug.Log("L'inventaire est plein!");
        return false;
      }
      inventory.Add(item);
      if (OnItemAdded != null) OnItemAdded(item);
      return true;
    }

    /// <summary>
    /// Retire un item de l'inventaire basé sur le ID
    /// </summary>
    /// <param name="itemID">Le ID de l'item à enlever</param>
    /// <returns>Si l'item était présent dans l'inventaire au moment de l'appel</returns>
    public bool RemoveItem(int itemID)
    {
      ItemInInventory[] allItems = GetAllItems();
      foreach (ItemInInventory item in allItems)
      {
        if (item.Item.ItemID == itemID)
        {
          RemoveItem(item);
          if (OnItemRemoved != null) OnItemRemoved(item);
          return true;
        }
      }
      return false;
    }
    /// <summary>
    /// Retire un item de l'inventaire. Si il s'agit de l'arme ou de l'armure équipée,
    /// l'enlèvera au préalable
    /// </summary>
    /// <param name="item">La référence de l'item à supprimer</param>
    /// <returns>Si l'item existe</returns>
    public bool RemoveItem(ItemInInventory item)
    {
      if (currentWeapon == item)
      {
        RemoveWeapon();
      }
      else if (currentArmor == item)
      {
        RemoveArmor();
      }
      bool itemWasFound = inventory.Remove(item);
      if (itemWasFound)
      {
        if (OnItemRemoved != null) OnItemRemoved(item);
      }
      return itemWasFound;
    }

    /// <summary>
    /// Retourne le premier item du type specifié. Retourne null si aucun item n'y correspond
    /// </summary>
    /// <param name="itemID">Le id de l'item</param>
    /// <returns>L'item</returns>
    public ItemInInventory GetItemOfID(int itemID)
    {
      ItemInInventory[] allItems = GetAllItems();
      foreach (ItemInInventory item in allItems)
      {
        if (item.Item.ItemID == itemID)
        {
          return item;
        }
      }
      return null;
    }

    #endregion

    #region WeaponAndArmor

    /// <summary>
    /// Retourne l'item d'arme que le joueur porte. Peut être null
    /// </summary>
    /// <returns>L'arme que le joueur porte</returns>
    public ItemInInventory GetWeaponItem()
    {
      return currentWeapon;
    }
    /// <summary>
    /// Retourne les numériques de l'arme que le joueur porte. Peut être null
    /// </summary>
    /// <returns>L'arme que le joueur porte</returns>
    public Weapon GetWeaponNumerics()
    {
      if (currentWeapon == null) return null;
      return currentWeapon.Item as Weapon;
    }
    /// <summary>
    /// Retourne le ID de l'arme que le joueur porte. Revoie -1 si le joueur ne porte pas
    /// d'arme
    /// </summary>
    /// <returns>L'arme que le joueur porte</returns>
    public int GetWeaponId()
    {
      if (currentWeapon == null) return -1;
      return currentWeapon.Item.ItemID;
    }
    /// <summary>
    /// Retourne le préfab attaché à l'arme que le joueur porte. 
    /// Peut être null si le joueur n'a pas d'arme ou que son arme
    /// ne possède pas de prefab d'attaque.
    /// </summary>
    /// <returns>L'arme que le joueur porte</returns>
    public GameObject GetWeaponPrefab()
    {
      if (currentWeapon == null) return null;
      return currentWeapon.AttackPrefab;
    }

    /// <summary>
    /// Retourne l'item d'armure que le joueur porte. Peut être null
    /// </summary>
    /// <returns>L'armure que le joueur porte</returns>
    public ItemInInventory GetArmorItem()
    {
      return currentArmor;
    }
    /// <summary>
    /// Retourne les numériques de l'armure que le joueur porte. Peut être null
    /// </summary>
    /// <returns>L'armure que le joueur porte</returns>
    public Armor GetArmorNumerics()
    {
      if (currentArmor == null) return null;
      return currentArmor.Item as Armor;
    }
    /// <summary>
    /// Retourne le ID de l'armure que le joueur porte. Revoie -1 si le joueur ne porte pas
    /// d'armure
    /// </summary>
    /// <returns>L'armure que le joueur porte</returns>
    public int GetArmorId()
    {
      if (currentArmor == null) return -1;
      return currentArmor.Item.ItemID;
    }


    /// <summary>
    /// Équipe une arme au joueur. Renvoie si l'arme à été équipée.
    /// </summary>
    /// <param name="item">L'arme à équiper</param>
    /// <param name="forceEquip">Équiper même si une arme est déjà équipée. True par défaut</param>
    /// <param name="ignoreLevelRequisite">Ignorer les requierement de niveau. False par défaut</param>
    /// <returns>Si l'arme a bel et bien été équipée</returns>
    public bool EquipWeapon(ItemInInventory item, bool forceEquip = true, bool ignoreLevelRequisite = false)
    {
      if (item == null || !(item.Item is Weapon))
      {
        return false;
      }
      Weapon weapon = (Weapon) (item.Item);
      if (!ignoreLevelRequisite)
      {
        if ( livingEntity.GetLevel() < weapon.RequiredLevel)
        {
          return false;
        }
      }
      if (currentWeapon != null && !forceEquip)
      {
        return false;
      }
      currentWeapon = item;
      if (OnWeaponChanged != null) OnWeaponChanged(item);
      return true;
    }

    /// <summary>
    /// Équipe une armure au joueur. Renvoie si l'armure à été équipée
    /// </summary>
    /// <param name="item">L'armure à équiper</param>
    /// <param name="forceEquip">Équiper même si une armure est déjà équipée. True par défaut</param>
    /// <param name="ignoreLevelRequisite">Ignorer les requierement de niveau. False par défaut</param>
    /// <returns>Si l'armure a bel et bien été équipée</returns>
    public bool EquipArmor(ItemInInventory item, bool forceEquip = true, bool ignoreLevelRequisite = false)
    {
      if (item == null || !(item.Item is Armor))
      {
        return false;
      }
      Armor armor = (Armor)(item.Item);
      if (!ignoreLevelRequisite)
      {
        if ( livingEntity.GetLevel() < armor.RequiredLevel)
        {
          return false;
        }
      }
      if (currentArmor != null && !forceEquip)
      {
        return false;
      }
      currentArmor = item;
      if (OnArmorChanged != null) OnArmorChanged(item);
      return true;
    }

    /// <summary>
    /// Retire l'arme au joueur. Elle restera dans son inventaire.
    /// </summary>
    public void RemoveWeapon()
    {
      //Ne pas trigger l'event si le joueur n'avait deja pas d'arme
      if (currentWeapon != null)
      {
        if (OnWeaponChanged != null) OnWeaponChanged(null);
      }
      currentWeapon = null;
    }
    /// <summary>
    /// Retire l'armure au joueur. Elle restera dans son inventaire.
    /// </summary>
    public void RemoveArmor()
    {
      //Ne pas trigger l'event si le joueur n'avait deja pas d'armure
      if (currentArmor != null)
      {
        if (OnArmorChanged != null) OnArmorChanged(null);
      }
      currentArmor = null;
    }
    #endregion

    #region Verifications

    /// <summary>
    /// Cette methode vérifie l'intégrité du système d'équippement. Va causer une erreur si le joueur équipe
    /// une pièce qu'il ne possède même pas.
    /// </summary>
    /// <param name="item">L'item nouvellement équipé</param>
    private void CheckIfEquiped(ItemInInventory item)
    {
      if (item != null && !HasItem(item))
      {
        //Debug.LogError("The equipped piece of equipment is not present in the inventory: is this an error?");
      }
    }

    #endregion

    #region OutsideInteraction

    //WISH: Mettre cette methode dans son propre component
    /// <summary>
    /// Tente de ramasser des items sur le sol
    /// </summary>
    public void Interact()
    {
      ItemOnFloorController[] allFloorItem = FindObjectsOfType<ItemOnFloorController>();
      ItemOnFloorController selectedItem = null;

      foreach (ItemOnFloorController itemOnFloor in allFloorItem)
      {
        if (itemOnFloor.GetComponent<Collider2D>().OverlapPoint(transform.position))
        {
          selectedItem = itemOnFloor;
          break;
        }
      }

      if (selectedItem != null)
      {
        selectedItem.PickUp(itemAdder);
      }
    }

    #endregion


  }
}

