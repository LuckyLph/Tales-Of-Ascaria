using System;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Un item est un objet pouvant être possédé par un joueur, un coffre ou un sac par terre.
  /// </summary>
  /// <remarks>
  /// Ne contient aucune icone ni autres attributs passés par référence, dans le but d'être fabilement
  /// enregistré dans la BD
  /// 
  /// A.Gagné
  /// </remarks>
  public abstract class Item : IDroppable, ICloneable
  {
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="name">Le nom de l'item</param>
    /// <param name="value">La valeur monétaire de l'item</param>
    /// <param name="rarity">La rareté de l'item</param>
    protected Item(int itemID, string name, int value, ItemRarity rarity)
    {
      ItemID = itemID;
      Name = name;
      Value = value;
      Rarity = rarity;
    }

    /// <summary>
    /// Constructeur avec conversion. Peut échouer si la rareté est hors de l'index d'ItemRarity
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="name">Le nom de l'item</param>
    /// <param name="value">La valeur monétaire de l'item</param>
    /// <param name="rarity">La rareté de l'item en int</param>
    protected Item(int itemID, string name, int value, int rarity)
    {
      ItemID = itemID;
      Name = name;
      Value = value;
      Rarity = IntToItemRarity(rarity);
    }

    /// <summary>
    /// Le constructeur clone. Utilise un autre Item
    /// </summary>
    /// <param name="clone">L'item à cloner</param>
    protected Item (Item clone)
    {
      ItemID = clone.ItemID;
      Name = clone.Name;
      Value = clone.Value;
      Rarity = clone.Rarity;
    }

    /// <summary>
    /// Le ID de l'item.
    /// </summary>
    /// <remarks>
    /// Ce ID ne représente ni le niveau ni la puissance de cet item. Ce chiffre représente l'item
    /// de base de cette variante. Utile pour savoir si il s'agit d'une épée, arc etc.
    /// </remarks>
    public int ItemID
    {
      get;
      private set;
    }
    /// <summary>
    /// Le nom de l'item
    /// </summary>
    public string Name
    {
      get;
      set;
    }
    /// <summary>
    /// La valeur monétaire de l'item
    /// </summary>
    public int Value
    {
      get;
      set;
    }
    /// <summary>
    /// La rareté de l'item
    /// </summary>
    public ItemRarity Rarity
    {
      get;
      set;
    }
    
    /// <summary>
    /// Créer un item par terre à la position demandée. Ne détruit pas l'item.
    /// </summary>
    /// <param name="position">Position</param>
    public virtual void Drop(Vector3 position)
    {
      GameObject.FindGameObjectWithTag(R.S.Tag.ApplicationDependencies).GetComponentInChildren<ItemOnFloorSpawner>().Spawn(this, position);
    }

    /// <summary>
    /// Créer une instance identique mais distincte de l'objet (nouvel espace mémoire)
    /// </summary>
    /// <returns>Une copie "deep"</returns>
    public abstract object Clone();

    /// <summary>
    /// Transforme un int en ItemRarity. Si il y a erreur, Common sera renvoyé
    /// </summary>
    /// <param name="input">Le chiffre</param>
    /// <returns>Le ItemRarity</returns>
    private ItemRarity IntToItemRarity(int input)
    {
      try
      {
        return (ItemRarity) input;
      }
      catch (Exception e)
      {
        Debug.LogError(e.ToString());
        return ItemRarity.Common;
      }
    }

    /// <summary>
    /// Retourne la couleur associée à une rareté
    /// </summary>
    /// <param name="rarity">La rareté</param>
    /// <returns>La couleur</returns>
    public static Color GetSpriteColorBasedOnRarity(ItemRarity rarity)
    {
      switch (rarity)
      {
        case ItemRarity.Common:
          return Color.white;
        case ItemRarity.Uncommon:
          return Color.green;
        case ItemRarity.Rare:
          return Color.blue;
        case ItemRarity.Very_Rare:
          return Color.magenta;
        case ItemRarity.Legendary:
          return new Color(1f, 0.4f, 0f);
        case ItemRarity.Mythic:
          return Color.red;
      }
      return Color.white;
    }
  }

  /// <summary>
  /// Représente un item qui se consume. 
  /// </summary>
  public sealed class Consumable : Item, IConsummable
  {
    /// <summary>
    /// La puissance de ce consumable.
    /// </summary>
    public float Power
    {
      get;
      set;
    }
    /// <summary>
    /// La description affichée au joueur de cet item
    /// </summary>
    public string Description
    {
      get;
      private set;
    }

    /// <summary>
    /// Le constructeur par défaut
    /// </summary>
    /// <param name="itemID">le ID de l'item</param>
    /// <param name="name">Le nom de l'item</param>
    /// <param name="value">La valeur monétaire de l'item</param>
    /// <param name="rarity">La rareté de l'item</param>
    /// <param name="power">La puissance du consommable</param>
    public Consumable(int itemID, string name, int value, ItemRarity rarity, float power, string description = "") : base(itemID, name, value, rarity)
    {
      Power = power;
      Description = description;
    }

    /// <summary>
    /// Le constructeur par défaut
    /// </summary>
    /// <param name="itemID">le ID de l'item</param>
    /// <param name="name">Le nom de l'item</param>
    /// <param name="value">La valeur monétaire de l'item</param>
    /// <param name="rarity">La rareté de l'item en int</param>
    /// <param name="power">La puissance du consommable</param>
    public Consumable(int itemID, string name, int value, int rarity, float power, string description = "") : base(itemID, name, value, rarity)
    {
      Power = power;
      Description = description;
    }
    /// <summary>
    /// Le constructeur clone
    /// </summary>
    /// <param name="clone">Un clone parfait</param>
    public Consumable(Consumable clone) : base(clone)
    {
      Power = clone.Power;
      Description = clone.Description;
    }

    /// <summary>
    /// Retourne un clone de l'item
    /// </summary>
    /// <returns>Le clone</returns>
    public override object Clone()
    {
      return new Consumable(this);
    }

    /// <summary>
    /// Applique les effets sur l'entité voulue. Ne supprime pas l'entité
    /// </summary>
    /// <param name="livingEntity">L'entité à affecter</param>
    /// <param name="effects">Les effets à appliquer</param>
    public void Use(LivingEntity livingEntity, Effect[] effects)
    {
      if (effects != null && effects.Length > 0)
      {
        foreach (Effect effect in effects)
        {
          AdjustEffect(effect);
          effect.ApplyOn(livingEntity);
        }
      }
    }

    /// <summary>
    /// Ajuste les chiffres de l'effet selon le power, si cela est logique
    /// </summary>
    /// <param name="effect">l'effet à modifier</param>
    private void AdjustEffect (Effect effect)
    {
      if (effect is InstantHeal)
      {
        ((InstantHeal)effect).SetHeal(Mathf.RoundToInt(Power));
      }
      if (effect is HealOverTime)
      {
        ((HealOverTime)effect).SetHeal(Mathf.RoundToInt(Power));
      }
      if (effect is ManaOverTime)
      {
        ((ManaOverTime)effect).SetMana(Mathf.RoundToInt(Power));
      }
      if (effect is DebuffImmunity)
      {
        ((DebuffImmunity)effect).duration = (Mathf.CeilToInt(Mathf.Pow(Power,3)));
      }
      if (effect is CrowdControlImmunity)
      {
        ((CrowdControlImmunity)effect).duration = (Mathf.CeilToInt(Mathf.Pow(Power, 3)));
      }
    }

    /// <summary>
    /// Retourne les stats de l'objet sous forme de string formatée. Utilise 3 lignes
    /// </summary>
    /// <returns>La string</returns>
    public override string ToString()
    {
      string formatedDescription = String.Format(Description, Mathf.RoundToInt(Power));
      string value =  (Name
              + "\nConsumable"
              + "\nWorth: " + Value + "$"
              + "\n\n" + formatedDescription);
      return value;
    }
  }

  /// <summary>
  /// Représente un item qui peut s'équiper
  /// </summary>
  /// <remarks>
  /// A.Gagné
  /// </remarks>
  public abstract class Equipable : Item
  {
    /// <summary>
    /// Le niveau requis pour équiper cet item
    /// </summary>
    public int RequiredLevel
    {
      get;
      set;
    }
    /// <summary>
    /// Le wisdom donné par cet équipement
    /// </summary>
    public int BonusWisdom
    {
      get;
      set;
    }
    /// <summary>
    /// Le strength donné par cet équipement
    /// </summary>
    public int BonusStrength
    {
      get;
      set;
    }

    /// <summary>
    /// Le constructeur par défaut
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="name">Le nom de l'item</param>
    /// <param name="value">La valeur monétaire de l'item</param>
    /// <param name="rarity">La rareté de l'item</param>
    /// <param name="level">Le niveau requis</param>
    protected Equipable(int itemID, string name, int value, ItemRarity rarity, int level, int bonusWisdom = 0, int bonusStrength = 0) : base(itemID, name, value, rarity)
    {
      RequiredLevel = level;
      BonusStrength = bonusStrength;
      BonusWisdom = bonusWisdom;
    }

    /// <summary>
    /// Le constructeur par défaut
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="name">Le nom de l'item</param>
    /// <param name="value">La valeur monétaire de l'item</param>
    /// <param name="rarity">La rareté de l'item en int</param>
    /// <param name="level">Le niveau requis</param>
    protected Equipable(int itemID, string name, int value, int rarity, int level, int bonusWisdom = 0, int bonusStrength = 0) : base(itemID, name, value, rarity)
    {
      RequiredLevel = level;
      BonusWisdom = bonusWisdom;
      BonusStrength = bonusStrength;
    }

    /// <summary>
    /// Constructeur clone
    /// </summary>
    /// <param name="clone">L'objet à cloner</param>
    protected Equipable(Equipable clone) : base(clone)
    {
      RequiredLevel = clone.RequiredLevel;
      BonusStrength = clone.BonusStrength;
      BonusWisdom = clone.BonusWisdom;
    }

  }

  /// <summary>
  /// Cette classe ne peut ni être utilisé ni être équipée
  /// </summary>
  /// <remarks>
  /// A.Gagné
  /// </remarks>
  public sealed class NonUsable : Item
  {
    /// <summary>
    /// La description affichée au joueur de cet item
    /// </summary>
    public string Description
    {
      get;
      private set;
    }

    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="name">Le nom de l'item</param>
    /// <param name="value">La valeur monétaire de l'item</param>
    /// <param name="rarity">La rareté de l'item</param>
    public NonUsable(int itemID, string name, int value, ItemRarity rarity, string description = "") : base(itemID, name, value, rarity)
    {
      Description = description;
    }
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="name">Le nom de l'item</param>
    /// <param name="value">La valeur monétaire de l'item</param>
    /// <param name="rarity">La rareté de l'item en int</param>
    public NonUsable(int itemID, string name, int value, int rarity, string description = "") : base(itemID, name, value, rarity)
    {
      Description = description;
    }

    /// <summary>
    /// Le constructeur clone
    /// </summary>
    /// <param name="clone">Le clone</param>
    public NonUsable(NonUsable clone) : base(clone)
    {
      Description = clone.Description;
    }


    /// <summary>
    /// Créer une instance identique mais distincte de l'objet (nouvel espace mémoire)
    /// </summary>
    /// <returns>Une copie "deep"</returns>
    public override object Clone()
    {
      return new NonUsable(this);
    }

    /// <summary>
    /// Retourne les stats de l'objet sous forme de string formatée. Utilise 3 lignes
    /// </summary>
    /// <returns>La string</returns>
    public override string ToString()
    {
      return (Name
              + "\nQuest Item (Cannot be equiped, activated nor consumed)"
              + "\nWorth: " + Value + "$"
              + "\n\n" + Description);
    }
  }

  /// <summary>
  /// Cette classe représente une arme. Doit être lié à un ItemInInventory pour
  /// être complète
  /// </summary>
  /// <remarks>
  /// A.Gagné
  /// </remarks>
  public sealed class Weapon : Equipable
  {

    /// <summary>
    /// Le dégat de base de l'arme (avant le scaling)
    /// </summary>
    public int BaseDamage
    {
      get;
      set;
    }
    /// <summary>
    /// Les attaques par seconde
    /// </summary>
    public float AttackPerSecond
    {
      get;
      set;
    }
    /// <summary>
    /// Le cooldown de l'action secondaire en secondes.
    /// </summary>
    public float SecondaryCooldown
    {
      get;
      set;
    }
    /// <summary>
    /// Le multiplicateur de wisdom à ajouter au dégats
    /// </summary>
    public float WisdomMultiplier
    {
      get;
      set;
    }
    /// <summary>
    /// Le multiplicateur de strength à ajouter au dégats
    /// </summary>
    public float StrengthMultiplier
    {
      get;
      set;
    }


    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="name">Le nom de l'arme</param>
    /// <param name="value">La valeur monétaire</param>
    /// <param name="rarity">La rareté de l'arme</param>
    /// <param name="level">Le niveau requis pour utiliser l'arme</param>
    /// <param name="baseDamage">Le dégat de base de l'arme</param>
    /// <param name="baseAttackSpeed">Les attaques par seconde de l'arme</param>
    /// <param name="secondaryCooldown">Le cooldown de l'action secondaire</param>
    /// <param name="wisdomMultiplier">Le multiplicateur de wisdom. 0 par défaut</param>
    /// <param name="strengthMultiplier">Le multiplicateur de strength. 0 par défaut</param>
    public Weapon(int itemID, string name, int value, ItemRarity rarity, int level, 
      int baseDamage, float baseAttackSpeed,float secondaryCooldown, float wisdomMultiplier = 0f, float strengthMultiplier = 0f,
      int bonusWisdom = 0, int bonusStrength = 0) : base(itemID, name, value, rarity, level,bonusWisdom,bonusStrength)
    {
      BaseDamage = baseDamage;
      AttackPerSecond = baseAttackSpeed;
      SecondaryCooldown = secondaryCooldown;
      WisdomMultiplier = wisdomMultiplier;
      StrengthMultiplier = strengthMultiplier;
    }

    /// <summary>
    /// Constructeur alternatif
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="name">Le nom de l'arme</param>
    /// <param name="value">La valeur monétaire</param>
    /// <param name="rarity">La rareté de l'arme en int</param>
    /// <param name="level">Le niveau requis pour utiliser l'arme</param>
    /// <param name="baseDamage">Le dégat de base de l'arme</param>
    /// <param name="baseAttackSpeed">Les attaques par seconde de l'arme</param>
    /// <param name="secondaryCooldown">Le cooldown de l'action secondaire</param>
    /// <param name="wisdomMultiplier">Le multiplicateur de wisdom. 0 par défaut</param>
    /// <param name="strengthMultiplier">Le multiplicateur de strength. 0 par défaut</param>
    public Weapon(int itemID, string name, int value, int rarity, int level,
      int baseDamage, float baseAttackSpeed,float secondaryCooldown, float wisdomMultiplier = 0f, float strengthMultiplier = 0f,
      int bonusWisdom = 0, int bonusStrength = 0) : base(itemID, name, value, rarity, level,bonusWisdom,bonusStrength)
    {
      BaseDamage = baseDamage;
      AttackPerSecond = baseAttackSpeed;
      SecondaryCooldown = secondaryCooldown;
      WisdomMultiplier = wisdomMultiplier;
      StrengthMultiplier = strengthMultiplier;
    }

    /// <summary>
    /// Constructeur copie
    /// </summary>
    /// <param name="clone">Le clone de l'arme</param>
    public Weapon(Weapon clone) : base(clone)
    {
      BaseDamage = clone.BaseDamage;
      AttackPerSecond = clone.AttackPerSecond;
      SecondaryCooldown = clone.SecondaryCooldown;
      WisdomMultiplier = clone.WisdomMultiplier;
      StrengthMultiplier = clone.StrengthMultiplier;
    }

    /// <summary>
    /// Clone l'instance de l'arme
    /// </summary>
    /// <returns>L'arme clonée</returns>
    public override object Clone()
    {
      return new Weapon(this);
    }

    /// <summary>
    /// Retourne les stats de l'arme sous forme de string formatée. Utilise 9 lignes
    /// </summary>
    /// <returns>La string</returns>
    public override string ToString()
    {
      return(Name
                + "\nWorth: " + Value + "$\n" + 
                "Damage: " + BaseDamage + "\n" +
                "Attack speed: " + AttackPerSecond + "\n" +
                "Secondary action cooldown: " + SecondaryCooldown + "\n" +
                "Scaling: " + StrengthMultiplier * 100f + "% STR, " + WisdomMultiplier * 100f + "% WIS\n" +
                "Strength+: " + BonusStrength + "\n" +
                "Wisdom+: " + BonusWisdom + "\n" +
                "Required Level: " + RequiredLevel);
    }

  }

  /// <summary>
  /// Représente une armure que le joueur peut équiper pour augmenter ses défenses.
  /// Doit être lié à un ItemInInventory pour
  /// </summary>
  /// <remarks>
  /// A.Gagné
  /// </remarks>
  public sealed class Armor : Equipable
  {
    /// <summary>
    /// La vie ajoutée par cette armure
    /// </summary>
    public int BonusHealth
    {
      get;
      set;
    }
    /// <summary>
    /// La mana ajoutée par cette armure
    /// </summary>
    public int BonusMana
    {
      get;
      set;
    }
    /// <summary>
    /// La constitution ajoutée par cette armure
    /// </summary>
    public int BonusConstitution
    {
      get;
      set;
    }
    /// <summary>
    /// Le spirit ajoutée par cette armure
    /// </summary>
    public int BonusSpirit
    {
      get;
      set;
    }


    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="name">Le nom de l'armure</param>
    /// <param name="value">La valeur monétaire de l'armure</param>
    /// <param name="rarity">La rareté de l'armure</param>
    /// <param name="level">Le niveau requis pour porter l'armure</param>
    public Armor(int itemID, string name, int value, ItemRarity rarity, int level, int bonusHealth, int bonusMana, int bonusConst,
      int bonusSpirit, int bonusWisdom = 0, int bonusStrength = 0) : base(itemID, name, value, rarity, level,bonusWisdom,bonusStrength)
    {
      BonusConstitution = bonusConst;
      BonusHealth = bonusHealth;
      BonusSpirit = bonusSpirit;
      BonusMana = bonusMana;
    }
    /// <summary>
    /// Constructeur alternatif
    /// </summary>
    /// <param name="itemID">Le ID de l'item</param>
    /// <param name="name">Le nom de l'armure</param>
    /// <param name="value">La valeur monétaire de l'armure</param>
    /// <param name="rarity">La rareté de l'armure en int</param>
    /// <param name="level">Le niveau requis pour porter l'armure</param>
    public Armor(int itemID, string name, int value, int rarity, int level, int bonusHealth, int bonusMana, int bonusConst,
      int bonusSpirit, int bonusWisdom = 0, int bonusStrength = 0) : base(itemID, name, value, rarity, level, bonusWisdom, bonusStrength)
    {
      BonusConstitution = bonusConst;
      BonusHealth = bonusHealth;
      BonusSpirit = bonusSpirit;
      BonusMana = bonusMana;
    }
    /// <summary>
    /// Le constructeur clone
    /// </summary>
    /// <param name="clone">Un clone de l'armure</param>
    public Armor(Armor clone) : base(clone)
    {
      BonusConstitution = clone.BonusConstitution;
      BonusHealth = clone.BonusHealth;
      BonusMana = clone.BonusMana;
      BonusSpirit = clone.BonusSpirit;
    }

    /// <summary>
    /// Retourne un clone "deep" de l'armure
    /// </summary>
    /// <returns>Le clone</returns>
    public override object Clone()
    {
      return new Armor(this);
    }

    /// <summary>
    /// Retourne les stats de l'armure sous forme de string formatée. Utilise 9 lignes
    /// </summary>
    /// <returns>La string</returns>
    public override string ToString()
    {
      return (Name
              + "\nWorth: " + Value + "$\n" +
              "HP+: " + BonusHealth + "\n" +
              "Mana+: " + BonusMana + "\n" +
              "Constitution+: " + BonusConstitution + "\n" +
              "Spirit+: " + BonusSpirit + "\n" +
              "Strength+: " + BonusStrength + "\n" +
              "Wisdom+: " + BonusWisdom + "\n" +
              "Required Level: " + RequiredLevel);
    }

  }
}
