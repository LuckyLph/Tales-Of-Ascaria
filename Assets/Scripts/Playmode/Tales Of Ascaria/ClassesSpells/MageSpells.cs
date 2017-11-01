using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class MageSpells : GameScript, ISpellClass
  {
    private LivingEntity livingEntity;

    [Tooltip("Spell prefab du hellfire surge")]
    [SerializeField] private GameObject hellfireSurge;
    [Tooltip("Spell prefab du impact")]
    [SerializeField] private GameObject impact;
    [Tooltip("Spell prefab du runic bolt")]
    [SerializeField] private GameObject runicBolt;
    [Tooltip("Spell prefab du earth surge")]
    [SerializeField] private GameObject earthSurge;

    [Tooltip("Stun appliqué lorsque hellfire surge est utilisé")]
    [SerializeField] private Stun hellfireSurgeStun;
    [Tooltip("Stun appliqué lorsque impact est utilisé")]
    [SerializeField] private Stun impactStun;
    [Tooltip("Stun appliqué lorsque runic bolt est utilisé")]
    [SerializeField] private Stun runicBoltStun;
    [Tooltip("Stun appliqué lorsque earth surge est utilisé")]
    [SerializeField] private Stun earthSurgeStun;

    private void Awake()
    {
      InjectDependencies("InjectMageSpells");
    }

    private void InjectMageSpells([EntityScope] LivingEntity livingEntity)
    {
      this.livingEntity = livingEntity;
    }

    /// <summary>
    /// Impact
    /// </summary>
    public void SpellX(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      GameObject impactClone = Instantiate(impact, playerPosition, Quaternion.identity);
      impactClone.GetComponentInChildren<ImpactController>().SetImpactParameters(statsSnapshot,transform.root);
      impactStun.ApplyOn(livingEntity);
    }
    /// <summary>
    /// Runic Bolt
    /// </summary>
    public void SpellY(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      GameObject runicBoltClone = Instantiate(runicBolt, playerPosition + playerDirection / 5, Quaternion.identity);
      runicBoltClone.GetComponentInChildren<RunicBoltController>().SetRunicBoltParameters(statsSnapshot,playerDirection);
      runicBoltStun.ApplyOn(livingEntity);
    }
    /// <summary>
    /// Hellfire Surge
    /// </summary>
    public void SpellA(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      GameObject hellfireSurgeClone = Instantiate(hellfireSurge, playerPosition + playerDirection / 5, Quaternion.identity);
      hellfireSurgeClone.GetComponentInChildren<HellfireSurgeController>().SetHellfireSurgeParameters(statsSnapshot, playerDirection);
      hellfireSurgeStun.ApplyOn(livingEntity);
    }
    /// <summary>
    /// earthSurge
    /// </summary>
    public void SpellB(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      GameObject earthSurgeClone = Instantiate(earthSurge, playerPosition, Quaternion.identity);
      earthSurgeClone.GetComponentInChildren<EarthSurgeController>().SetEarthSurgeParameters(statsSnapshot, playerDirection);
      earthSurgeStun.ApplyOn(livingEntity);
    }
  }
}
