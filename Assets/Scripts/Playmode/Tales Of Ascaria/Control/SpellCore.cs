using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  ///   Spellcore represente la classe qui envoie les appels de sorts, qui gère les cooldowns et la mana
  /// </summary>
  public class SpellCore : GameScript
  {
    /// <summary>
    /// Lorsque le cooldown est terminé
    /// </summary>
    /// <param name="spellRefreshed">Le sort prêt</param>
    public delegate void CooldownEventHandler(Spell spellRefreshed);

    /// <summary>
    /// les sorts disponibles
    /// </summary>
    public enum Spell
    {
      Y,
      B,
      A,
      X,
      Primary,
      Secondary
    }

    /// <summary>
    /// Temps avant de désactiver le spell mode
    /// </summary>
    private const float TimeBeforeConsideringSpellModeDisabled = 0.048f;

    /// <summary>
    /// L'index du primary dans les tableaux
    /// </summary>
    private const int PrimaryIndex = 4;

    /// <summary>
    /// L'index du secondary dans les tableaux
    /// </summary>
    private const int SecondaryIndex = PrimaryIndex + 1;


    private int[] spellManaCosts;
    private float[] spellCooldowns;
    private bool[] cooldownNotified;
    private bool pressingSpellButton;
    private float stopInputAt;
    private bool canUsePrimaryAndSecondary;

    private Inventory inventory;
    private LivingEntity livingEntity;
    private PlayerController playerController;
    private ISpellClass spellClass;
    private WeaponSpawnerController weaponController;

    #region InspectorData

    [Header("Mana Costs")]
    [Tooltip("Le cout en mana du sort Y")]
    [SerializeField]
    private int ySpellManaCost;

    [Tooltip("Le cout en mana du sort B")]
    [SerializeField]
    private int bSpellManaCost;

    [Tooltip("Le cout en mana du sort A")]
    [SerializeField]
    private int aSpellManaCost;

    [Tooltip("Le cout en mana du sort X")]
    [SerializeField]
    private int xSpellManaCost;

    [Space(10)]
    [Header("Cooldowns")]
    [Tooltip("Le cooldown du sort Y")]
    [SerializeField]
    private float ySpellCooldown;

    [Tooltip("Le cooldown du sort B")]
    [SerializeField]
    private float bSpellCooldown;

    [Tooltip("Le cooldown du sort A")]
    [SerializeField]
    private float aSpellCooldown;

    [Tooltip("Le cooldown du sort X")]
    [SerializeField]
    private float xSpellCooldown;

    #endregion


    /// <summary>
    ///   Les floats qui mémorisent la dernière utilisation des sorts afin de gérer le cooldowns
    /// </summary>
    private float[] timestamps;

    /// <summary>
    /// Lorsqu'un cooldown se termine
    /// </summary>
    public event CooldownEventHandler OnCooldownOver;

    private void InjectSpellcore([EntityScope] PlayerController playerController,
                                 [EntityScope] LivingEntity livingEntity,
                                 [EntityScope] Inventory inventory,
                                 [EntityScope] ISpellClass spellClass,
                                 [EntityScope] WeaponSpawnerController weaponSpawner)
    {
      this.playerController = playerController;
      this.livingEntity = livingEntity;
      this.inventory = inventory;
      this.spellClass = spellClass;
      weaponController = weaponSpawner;
    }

    private void Awake()
    {
      InjectDependencies("InjectSpellcore");

      spellCooldowns = new float[6];
      spellManaCosts = new int[6];

      spellCooldowns[0] = ySpellCooldown;
      spellCooldowns[1] = bSpellCooldown;
      spellCooldowns[2] = aSpellCooldown;
      spellCooldowns[3] = xSpellCooldown;

      spellManaCosts[0] = ySpellManaCost;
      spellManaCosts[1] = bSpellManaCost;
      spellManaCosts[2] = aSpellManaCost;
      spellManaCosts[3] = xSpellManaCost;

      cooldownNotified = new bool[spellCooldowns.Length];
      timestamps = new float[spellCooldowns.Length];
      for (var i = 0; i < cooldownNotified.Length; i++)
      {
        cooldownNotified[i] = true;
      }

      inventory.OnWeaponChanged += AdjustToWeaponStats;
    }

    private void OnDestroy()
    {
      inventory.OnWeaponChanged -= AdjustToWeaponStats;
    }

    /// <summary>
    ///   Nous allons mettre un bool a vrai afin de savoir que l'on appuie sur le boutton
    /// </summary>
    public void HandleSpellButton()
    {
      pressingSpellButton = true;
      stopInputAt = Time.time + TimeBeforeConsideringSpellModeDisabled;
    }

    private void LateUpdate()
    {
      if (pressingSpellButton && Time.time > stopInputAt)
        pressingSpellButton = false;
    }

    private void Update()
    {
      for (var i = 0; i < spellCooldowns.Length; i++)
        if (!cooldownNotified[i])
          if (Time.time > timestamps[i] + spellCooldowns[i])
          {
            cooldownNotified[i] = true;
            if (OnCooldownOver != null) OnCooldownOver((Spell)i);
          }
    }

    /// <summary>
    ///   Le joueur peut-il utiliser des sorts?
    /// </summary>
    /// <returns></returns>
    private bool PlayerCanUseSpell()
    {
      //WISH: Utiliser le living entity
      return true; //!livingEntity.IsAffectedByAlterationState(typeof(Stun));
    }

    /// <summary>
    /// Change le cooldown et le mana cost des sorts Primary et Secondary en se basant
    /// sur l'arme passé en paramètre. S'arrête si ce n'est pas une arme
    /// </summary>
    /// <param name="weapon">L'arme du joueur. Peut etre null</param>
    private void AdjustToWeaponStats(ItemInInventory weapon)
    {
      if (weapon == null)
      {
        canUsePrimaryAndSecondary = false;
        return;
      }
      canUsePrimaryAndSecondary = true;
      Weapon weaponNumerics = (Weapon)weapon.Item;
      spellCooldowns[PrimaryIndex] = 1f / weaponNumerics.AttackPerSecond;
      spellManaCosts[PrimaryIndex] = 0;

      spellCooldowns[SecondaryIndex] = weaponNumerics.SecondaryCooldown;
      spellManaCosts[SecondaryIndex] = 0;
    }

    #region SpellActivators

    /// <summary>
    ///   Si c'est approprié, déclenche le sort
    /// </summary>
    public void SpellPressedY(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (!pressingSpellButton)
        return;
      const int tabIndex = 0;
      if (Time.time >= spellCooldowns[tabIndex] + timestamps[tabIndex])
        if (livingEntity.CanCastSpell(spellManaCosts[tabIndex]))
        {
          spellClass.SpellY(statsSnapshot, playerDirection, playerPosition);
        }
    }

    /// <summary>
    ///   Si c'est approprié, déclenche le sort
    /// </summary>
    public void SpellPressedB(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (!pressingSpellButton)
        return;
      const int tabIndex = 1;
      if (Time.time >= spellCooldowns[tabIndex] + timestamps[tabIndex])
      {
        if (livingEntity.CanCastSpell(spellManaCosts[tabIndex]))
        {
          if (!pressingSpellButton)
            return;
          if (Time.time >= spellCooldowns[tabIndex] + timestamps[tabIndex])
          {
            if (livingEntity.CanCastSpell(spellManaCosts[tabIndex]))
            {
              livingEntity.ReduceMana(spellManaCosts[tabIndex]);
              timestamps[tabIndex] = Time.time;
              cooldownNotified[tabIndex] = false;
              if (PlayerCanUseSpell())
              {
                spellClass.SpellB(statsSnapshot, playerDirection, playerPosition);
              }
            }
          }
        }
      }
    }

    /// <summary>
    ///   Si c'est approprié, déclenche le sort
    /// </summary>
    public void SpellPressedA(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (!pressingSpellButton)
        return;
      const int tabIndex = 2;
      if (Time.time >= spellCooldowns[tabIndex] + timestamps[tabIndex])
        if (livingEntity.CanCastSpell(spellManaCosts[tabIndex]))
        {
          livingEntity.ReduceMana(spellManaCosts[tabIndex]);
          cooldownNotified[tabIndex] = false;
          timestamps[tabIndex] = Time.time;
          if (PlayerCanUseSpell())
          {
            spellClass.SpellA(statsSnapshot, playerDirection, playerPosition);
          }
        }
    }

    /// <summary>
    ///   Si c'est approprié, déclenche le sort
    /// </summary>
    public void SpellPressedX(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (!pressingSpellButton)
        return;
      const int tabIndex = 3;
      if (Time.time >= spellCooldowns[tabIndex] + timestamps[tabIndex])
        if (livingEntity.CanCastSpell(spellManaCosts[tabIndex]))
        {
          livingEntity.ReduceMana(spellManaCosts[tabIndex]);
          cooldownNotified[tabIndex] = false;
          timestamps[tabIndex] = Time.time;
          if (PlayerCanUseSpell())
          {
            spellClass.SpellX(statsSnapshot, playerDirection, playerPosition);
          }
        }
    }

    /// <summary>
    ///   Si c'est approprié, déclenche l'attaque primaire
    /// </summary>
    public void SpellPressedPrimary(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (pressingSpellButton || !canUsePrimaryAndSecondary)
        return;
      const int tabIndex = 4;
      if (Time.time >= spellCooldowns[tabIndex] + timestamps[tabIndex])
        if (livingEntity.CanCastSpell(spellManaCosts[tabIndex]))
        {
          livingEntity.ReduceMana(spellManaCosts[tabIndex]);
          cooldownNotified[tabIndex] = false;
          timestamps[tabIndex] = Time.time;
          weaponController.PrimaryAction(statsSnapshot, playerDirection, playerPosition);
        }
    }

    /// <summary>
    ///   Si c'est approprié, déclenche l'attaque secondaire
    /// </summary>
    public void SpellPressedSecondary(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (pressingSpellButton || !canUsePrimaryAndSecondary)
        return;
      const int tabIndex = 5;
      if (Time.time >= spellCooldowns[tabIndex] + timestamps[tabIndex])
        if (livingEntity.CanCastSpell(spellManaCosts[tabIndex]))
        {
          livingEntity.ReduceMana(spellManaCosts[tabIndex]);
          cooldownNotified[tabIndex] = false;
          timestamps[tabIndex] = Time.time;
          weaponController.SecondaryAction(statsSnapshot, playerDirection, playerPosition);
        }
    }

    #endregion
  }
}