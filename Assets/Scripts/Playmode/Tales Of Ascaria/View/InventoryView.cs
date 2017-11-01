using Harmony;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

namespace TalesOfAscaria
{
  /// <summary>
  /// Ce composant affichera un inventaire, selon les paramètres indiqués
  /// </summary>
  /// <remarks>
  /// A.Gagné
  /// </remarks>
  public class InventoryView : GameScript
  {
    /// <summary>
    /// Le nombre d'item qui ne rentrerait pas dans le UI d'inventaire
    /// </summary>
    public const int MaximumItemsDisplayed = 18;

    [Space(10)] [Tooltip("Le joueur qui sera utilisé pour voir et naviguer dans l'inventaire")] [SerializeField]
    private PlayerIndex listenedPlayer;


    [Space(10)] [Tooltip("L'icone lorsqu'un item est meilleur")] [SerializeField] private Sprite betterSprite;
    [Space(10)] [Tooltip("L'icone lorsqu'un item est égal")] [SerializeField] private Sprite equalSprite;
    [Space(10)] [Tooltip("L'icone lorsqu'un item est pire")] [SerializeField] private Sprite worstSprite;


    private Inventory inventoryToDisplay;
    private GameObject displayRoot;
    private ItemInInventory selectedItem;
    private int selectedIndex;
    private ItemInInventory[] allItems;

    private Text itemDetails;
    private Image[] itemImages;
    private Image weaponView;
    private Image armorView;
    private Image[] statsComparators;


    /// <summary>
    /// Détermine si ce View affichera l'inventaire. Utile si le aucun joueur n'est associé à ce View
    /// </summary>
    public bool Active { get; set; }

    private PlayerInputSensor inputSensor;
    private LivingEntity linkedLivingEntity;
    private bool opened;

    /// <summary>
    /// L'inventaire est-il ouvert en ce moment?
    /// </summary>
    public bool Displayed
    {
      get { return opened; }
    }

    private void Awake()
    {
      InjectDependencies("InjectInventoryView");
      SetListenedInventory();
      statsComparators = new Image[9];
      if (inventoryToDisplay == null)
      {
        Debug.LogError("L'inventaire est nul");
      }
      linkedLivingEntity = inventoryToDisplay.GetComponent<LivingEntity>();
      if (linkedLivingEntity == null)
      {
        Debug.LogError("L'objet d'inventaire ne possède pas de living entity");
      }
      displayRoot = transform.Find("display").gameObject;
      itemImages = new Image[MaximumItemsDisplayed];
      SetAllUILinks();

      inputSensor.Players[(int) listenedPlayer].OnToggleMenu += ToggleInventory;
      inputSensor.Players[(int) listenedPlayer].OnConfirm += TryToEquip;
      inputSensor.Players[(int) listenedPlayer].OnConfirm += TryToConsume;
      inputSensor.Players[(int) listenedPlayer].OnButtonBPressed += TryToDropItem;
      inputSensor.Players[(int) listenedPlayer].OnUp += MoveUp;
      inputSensor.Players[(int) listenedPlayer].OnDown += MoveDown;
      inputSensor.Players[(int) listenedPlayer].OnLeft += MoveLeft;
      inputSensor.Players[(int) listenedPlayer].OnRight += MoveRight;


      Active = true;
      CloseInventory();
    }

    private void OnDestroy()
    {
      inputSensor.Players[(int)listenedPlayer].OnToggleMenu -= ToggleInventory;
      inputSensor.Players[(int)listenedPlayer].OnConfirm -= TryToEquip;
      inputSensor.Players[(int)listenedPlayer].OnConfirm -= TryToConsume;
      inputSensor.Players[(int)listenedPlayer].OnButtonBPressed -= TryToDropItem;
      inputSensor.Players[(int)listenedPlayer].OnUp -= MoveUp;
      inputSensor.Players[(int)listenedPlayer].OnDown -= MoveDown;
      inputSensor.Players[(int)listenedPlayer].OnLeft -= MoveLeft;
      inputSensor.Players[(int)listenedPlayer].OnRight -= MoveRight;
    }

    private void InjectInventoryView([ApplicationScope] PlayerInputSensor sensor)
    {
      inputSensor = sensor;
    }

    /// <summary>
    /// Ouvre et ferme l'inventaire
    /// </summary>
    private void ToggleInventory()
    {
      if (Active)
      {
        if (opened)
        {
          CloseInventory();
        }
        else
        {
          OpenInventory();
        }
        opened = !opened;
      }
    }

    /// <summary>
    /// Ouvre l'inventaire
    /// </summary>
    private void OpenInventory()
    {
      selectedItem = null;
      selectedIndex = 0;
      allItems = inventoryToDisplay.GetAllItems();
      displayRoot.SetActive(true);

      UpdateDisplay();
    }

    /// <summary>
    /// Ferme l'inventaire
    /// </summary>
    private void CloseInventory()
    {
      displayRoot.SetActive(false);
    }

    /// <summary>
    /// Met l'affichage à jour
    /// </summary>
    private void UpdateDisplay()
    {
      for (int i = 0; i < MaximumItemsDisplayed; i++)
      {
        itemImages[i].transform.Find(R.S.GameObject.SelectedUi).GetComponent<Image>().enabled = (selectedIndex - 1 == i);
        if (i >= allItems.Length)
        {
          itemImages[i].color = Color.black;
          itemImages[i].sprite = null;
          itemImages[i].transform.Find(R.S.GameObject.WeaponEquipedUi).GetComponent<Image>().enabled = false;
          itemImages[i].transform.Find(R.S.GameObject.ArmorEquipedUi).GetComponent<Image>().enabled = false;
          continue;
        }
        itemImages[i].sprite = allItems[i].Sprite;
        itemImages[i].color = Item.GetSpriteColorBasedOnRarity(allItems[i].Item.Rarity);
      }
      int index = 0;
      foreach (ItemInInventory item in allItems)
      {
        itemImages[index].transform.Find(R.S.GameObject.WeaponEquipedUi).GetComponent<Image>().enabled =
          (item == inventoryToDisplay.GetWeaponItem());
        itemImages[index++].transform.Find(R.S.GameObject.ArmorEquipedUi).GetComponent<Image>().enabled =
          (item == inventoryToDisplay.GetArmorItem());
      }

      if (selectedItem != null)
      {
        itemDetails.text = selectedItem.Item.ToString();
      }
      else
      {
        itemDetails.text = "No item selected";
      }

      SetWeaponAndArmorIcons();
      SetComparators();
    }

    /// <summary>
    /// Tente d'équiper l'item sélectionné
    /// </summary>
    private void TryToEquip()
    {
      
      if (opened && selectedItem != null)
      {
        SetSelectedItemBasedOnIndex();
        if (selectedItem == inventoryToDisplay.GetWeaponItem())
        {
          inventoryToDisplay.RemoveWeapon();
          UpdateDisplay();
          return;
        }
        if (selectedItem == inventoryToDisplay.GetArmorItem())
        {
          inventoryToDisplay.RemoveArmor();
          UpdateDisplay();
          return;
        }
        inventoryToDisplay.EquipWeapon(selectedItem);
        inventoryToDisplay.EquipArmor(selectedItem);
        UpdateDisplay();
      }
    }

    /// <summary>
    /// Tente de consommer l'item sélectionné
    /// </summary>
    private void TryToConsume()
    {
      if (opened && selectedItem != null && selectedItem.Item is Consumable)
      {
        if (inventoryToDisplay.RemoveItem(selectedItem))
        {
          ((Consumable) selectedItem.Item).Use(linkedLivingEntity, selectedItem.EffectUponConsumption);
          SetSelectedItemBasedOnIndex();
          UpdateDisplay();
        }
      }
    }

    /// <summary>
    /// Tente de déposer l'item sélectionné
    /// </summary>
    private void TryToDropItem()
    {
      
      if (opened && selectedItem != null)
      {
        SetSelectedItemBasedOnIndex();
        if (inventoryToDisplay.RemoveItem(selectedItem))
        {
          selectedItem.Item.Drop(inventoryToDisplay.transform.position);
          SetSelectedItemBasedOnIndex();
          UpdateDisplay();
        }
      }
    }

    /// <summary>
    /// Bouge le curseur de l'inventaire vers la gauche
    /// </summary>
    private void MoveLeft()
    {
      if (!opened)
      {
        return;
      }
      if (selectedIndex == 0)
      {
        selectedIndex = 1;
      }
      else if (selectedIndex == 1 || selectedIndex == 7 || selectedIndex == 13)
      {
        selectedIndex += ((MaximumItemsDisplayed / 3) - 1);
      }
      else
      {
        selectedIndex--;
      }
      SetSelectedItemBasedOnIndex();
    }

    /// <summary>
    /// Bouge le curseur de l'inventaire vers la droite
    /// </summary>
    private void MoveRight()
    {
      if (!opened)
      {
        return;
      }
      if (selectedIndex == 0)
      {
        selectedIndex = 1;
      }
      else if (selectedIndex == 6 || selectedIndex == 12 || selectedIndex == 18)
      {
        selectedIndex -= ((MaximumItemsDisplayed / 3) - 1);
      }
      else
      {
        selectedIndex++;
      }
      SetSelectedItemBasedOnIndex();
    }

    /// <summary>
    /// Bouge le curseur de l'inventaire vers le haut
    /// </summary>
    private void MoveUp()
    {
      if (!opened)
      {
        return;
      }
      if (selectedIndex == 0)
      {
        selectedIndex = 1;
      }
      else if (selectedIndex <= MaximumItemsDisplayed / 3)
      {
        selectedIndex += 2 * (MaximumItemsDisplayed / 3);
      }
      else
      {
        selectedIndex -= (MaximumItemsDisplayed / 3);
      }
      SetSelectedItemBasedOnIndex();
    }

    /// <summary>
    /// Bouge le curseur de l'inventaire vers le bas
    /// </summary>
    private void MoveDown()
    {
      if (!opened)
      {
        return;
      }
      if (selectedIndex == 0)
      {
        selectedIndex = 1;
      }
      else if (selectedIndex >= (2 * (MaximumItemsDisplayed / 3)) + 1)
      {
        selectedIndex -= 2 * (MaximumItemsDisplayed / 3);
      }
      else
      {
        selectedIndex += (MaximumItemsDisplayed / 3);
      }
      SetSelectedItemBasedOnIndex();
    }

    /// <summary>
    /// Retourne le sprite correspondant à la comparaison de deux valeurs
    /// </summary>
    /// <param name="first">La première valeur</param>
    /// <param name="second">La deuxième</param>
    /// <param name="higherIsBetter">Plus haut est-il meilleur? True par défaut</param>
    /// <returns>La sprite correspondante</returns>
    private Sprite GetComparatorSprite(float first, float second, bool higherIsBetter = true)
    {
      if (first == second)
      {
        return equalSprite;
      }
      if ((first > second) == higherIsBetter)
      {
        return betterSprite;
      }
      return worstSprite;
    }

    /// <summary>
    /// Met à jour l'image représentant l'arme et l'armure équipée
    /// </summary>
    private void SetWeaponAndArmorIcons()
    {
      ItemInInventory weapon = inventoryToDisplay.GetWeaponItem();
      ItemInInventory armor = inventoryToDisplay.GetArmorItem();
      if (weapon == null)
      {
        weaponView.color = Color.black;
        weaponView.sprite = null;
      }
      else
      {
        weaponView.sprite = weapon.Sprite;
        weaponView.color = Item.GetSpriteColorBasedOnRarity(weapon.Item.Rarity);
      }
      if (armor == null)
      {
        armorView.color = Color.black;
        armorView.sprite = null;
      }
      else
      {
        armorView.sprite = armor.Sprite;
        armorView.color = Item.GetSpriteColorBasedOnRarity(armor.Item.Rarity);
      }
    }

    /// <summary>
    /// Met à jour les icones de comparaison entre la pièce sélectionnée et équipée
    /// </summary>
    private void SetComparators()
    {
      #region Weapon

      Weapon equipedWeapon = inventoryToDisplay.GetWeaponNumerics();
      Armor equipedArmor = inventoryToDisplay.GetArmorNumerics();
      if (equipedWeapon != null && selectedItem != null && selectedItem.Item is Weapon)
      {
        Weapon selectedWeapon = (Weapon) selectedItem.Item;
        statsComparators[0].enabled = true;
        statsComparators[0].sprite = GetComparatorSprite(selectedWeapon.Value, equipedWeapon.Value);
        statsComparators[1].enabled = true;
        statsComparators[1].sprite = GetComparatorSprite(selectedWeapon.BaseDamage, equipedWeapon.BaseDamage);
        statsComparators[2].enabled = true;
        statsComparators[2].sprite = GetComparatorSprite(selectedWeapon.AttackPerSecond, equipedWeapon.AttackPerSecond);
        statsComparators[3].enabled = true;
        statsComparators[3].sprite = GetComparatorSprite(selectedWeapon.SecondaryCooldown,
          equipedWeapon.SecondaryCooldown, false);
        statsComparators[4].enabled = true;
        statsComparators[4].sprite = GetComparatorSprite(
          selectedWeapon.StrengthMultiplier + selectedWeapon.WisdomMultiplier
          , equipedWeapon.StrengthMultiplier + equipedWeapon.WisdomMultiplier);
        statsComparators[5].enabled = true;
        statsComparators[5].sprite = GetComparatorSprite(selectedWeapon.BonusStrength, equipedWeapon.BonusStrength);
        statsComparators[6].enabled = true;
        statsComparators[6].sprite = GetComparatorSprite(selectedWeapon.BonusWisdom, equipedWeapon.BonusWisdom);
        statsComparators[7].enabled = true;
        statsComparators[7].sprite = GetComparatorSprite(selectedWeapon.RequiredLevel, equipedWeapon.RequiredLevel);
      }

      #endregion

      #region Armor

      else if (equipedArmor != null && selectedItem != null && selectedItem.Item is Armor)
      {
        Armor selectedArmor = (Armor) selectedItem.Item;
        statsComparators[0].enabled = true;
        statsComparators[0].sprite = GetComparatorSprite(selectedArmor.Value, equipedArmor.Value);
        statsComparators[1].enabled = true;
        statsComparators[1].sprite = GetComparatorSprite(selectedArmor.BonusHealth, equipedArmor.BonusHealth);
        statsComparators[2].enabled = true;
        statsComparators[2].sprite = GetComparatorSprite(selectedArmor.BonusMana, equipedArmor.BonusMana);
        statsComparators[3].enabled = true;
        statsComparators[3].sprite =
          GetComparatorSprite(selectedArmor.BonusConstitution, equipedArmor.BonusConstitution);
        statsComparators[4].enabled = true;
        statsComparators[4].sprite = GetComparatorSprite(selectedArmor.BonusSpirit, equipedArmor.BonusSpirit);
        statsComparators[5].enabled = true;
        statsComparators[5].sprite = GetComparatorSprite(selectedArmor.BonusStrength, equipedArmor.BonusStrength);
        statsComparators[6].enabled = true;
        statsComparators[6].sprite = GetComparatorSprite(selectedArmor.BonusWisdom, equipedArmor.BonusWisdom);
        statsComparators[7].enabled = true;
        statsComparators[7].sprite = GetComparatorSprite(selectedArmor.RequiredLevel, equipedArmor.RequiredLevel);
      }
      else
      {
        foreach (Image comparator in statsComparators)
        {
          comparator.enabled = false;
        }
      }

      #endregion
    }

    #region Setup

    /// <summary>
    /// S'injecte l'inventaire à écouter
    /// </summary>
    private void SetListenedInventory()
    {
      inventoryToDisplay = GameObject.FindGameObjectWithTag("Player" + ((int) listenedPlayer + 1))
        .GetComponentInChildren<Inventory>();
      inventoryToDisplay.transform.root.GetComponentInChildren<PlayerController>().InventoryView = this;
      inventoryToDisplay.OnItemAdded += HandleItemMoved;
      inventoryToDisplay.OnItemRemoved += HandleItemMoved;
    }

    /// <summary>
    /// Le lie aux images contenant le UI
    /// </summary>
    private void SetAllUILinks()
    {
      itemDetails = transform.Find("display").Find("itemDetails").Find("Text").GetComponent<Text>();
      for (int i = 1; i <= MaximumItemsDisplayed; i++)
      {
        itemImages[i - 1] = transform.Find("display").Find("Item (" + i + ")").GetComponent<Image>();
      }

      armorView = transform.Find(R.S.GameObject.Display).Find(R.S.GameObject.Armor).GetComponent<Image>();
      weaponView = transform.Find(R.S.GameObject.Display).Find(R.S.GameObject.Weapon).GetComponent<Image>();

      for (int i = 0; i < 9; i++)
      {
        statsComparators[i] = transform.Find(R.S.GameObject.Display).Find(R.S.GameObject.ItemDetails)
          .Find("comparator (" + i + ")").GetComponent<Image>();
      }
    }

    /// <summary>
    /// Set l'item basé sur l'index de sélection
    /// </summary>
    void SetSelectedItemBasedOnIndex()
    {
      if ((selectedIndex - 1) >= allItems.Length)
      {
        selectedItem = null;
      }
      else if (selectedIndex > 0)
      {
        selectedItem = allItems[selectedIndex - 1];
      }
      else
      {
        //OPTIMISATION STFU LOUIS
        return;
      }
      UpdateDisplay();
    }

    /// <summary>
    /// Prend en charge l'event lorsqu'un item est ajouté ou retiré
    /// </summary>
    /// <param name="item">L'item ajouté / retiré</param>
    void HandleItemMoved(ItemInInventory item)
    {
      allItems = inventoryToDisplay.GetAllItems();
      
      UpdateDisplay();
    }

    #endregion
  }
}