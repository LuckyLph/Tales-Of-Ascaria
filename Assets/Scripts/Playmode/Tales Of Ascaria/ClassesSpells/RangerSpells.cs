using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class RangerSpells : GameScript, ISpellClass
  {
    [Tooltip("Prefab du spell death lotus")]
    [SerializeField] private GameObject deathLotus;

    [Tooltip("The Fan of Blade's blade prefab")]
    [SerializeField] private GameObject fanOfBlades;

    [Tooltip("The Vault's prefab")]
    [SerializeField] private GameObject vault;

    [Tooltip("The Vault's dash")]
    [SerializeField] private Dash vaultDash;

    [Tooltip("The Shadow Pursuit's invisibility stats modifier")]
    [SerializeField] private StatsModifier shadowPursuitInvisibilityModifier;

    [Tooltip("The Shadow Pursuit's combat stats modifier")]
    [SerializeField] private StatsModifier shadowPursuitCombatModifier;

    [Tooltip("Stun appliqué sur le ranger lorsqu'il utilise fan of blades")]
    [SerializeField] private Stun fanOfBladesStun;

    [Tooltip("Stun appliqué sur le ranger lorsqu'il utilise death lotus")]
    [SerializeField] private Stun deathLotusStun;

    private LivingEntity playerEntity;

    private void InjectRangerSpells([EntityScope] LivingEntity playerEntity)
    {
      this.playerEntity = playerEntity;
    }

    private void Awake()
    {
      InjectDependencies("InjectRangerSpells");
    }

    public void SpellX(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (playerEntity.IsInvisible)
      {
        playerEntity.IsInvisible = false;
      }
      if (playerEntity.IsInvisible)
      {
        playerEntity.IsInvisible = false;
      }
      fanOfBladesStun.ApplyOn(playerEntity);
      GameObject fanOfBladesClone = Instantiate(fanOfBlades, gameObject.transform.position, Quaternion.identity);
      StartCoroutine(fanOfBladesClone.GetComponentInChildren<FanOfBladesController>().
        SetFanOfBladesParameters(playerEntity.GetStats().GetStatsSnapshot(), playerDirection, playerPosition));
    }


    public void SpellY(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      shadowPursuitInvisibilityModifier.ApplyOn(playerEntity);
      shadowPursuitCombatModifier.ApplyOn(playerEntity);
    }


    public void SpellA(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (playerEntity.IsInvisible)
      {
        playerEntity.IsInvisible = false;
      }
      Debug.Log("Vault launched!");
      vaultDash.DashDirection = playerDirection;
      vaultDash.ApplyOn(playerEntity);

      GameObject vaultClone = Instantiate(vault, transform.root.position + (Vector3)playerDirection / 10, Quaternion.identity);
      vaultClone.transform.parent = transform.root;
      vaultClone.transform.position = playerPosition + playerDirection / 10 + Vector2.down / 10;
      vaultClone.GetComponentInChildren<VaultController>().SetVaultParameters(playerEntity.transform.root, statsSnapshot, playerEntity, vaultClone);
    }

    public void SpellB(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      deathLotusStun.ApplyOn(playerEntity);
      GameObject deathLotusClone = Instantiate(deathLotus, gameObject.transform.position, Quaternion.identity);
      deathLotusClone.GetComponentInChildren<DeathLotusController>().
        SetDeathLotusParameters(playerEntity.GetStats().GetStatsSnapshot(), transform.parent);
    }

    public void StopVaultDash()
    {
      vaultDash.StopDash();
    }
  }
}
