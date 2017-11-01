using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class GuardianSpells : GameScript, ISpellClass
  {
    [Tooltip("Spell prefab du high guard")]
    [SerializeField] private GameObject highGuard;
    [Tooltip("Spell prefab du bash")]
    [SerializeField] private GameObject bash;
    [Tooltip("Spell prefab du devastating lunge")]
    [SerializeField] private GameObject devastatingLunge;
    [Tooltip("Spell prefab du provoke")]
    [SerializeField] private GameObject provoke;

    [Tooltip("Stun appliqué lorsque bash est utilisé")]
    [SerializeField] private Stun bashStun;
    [Tooltip("Stun appliqué lorsque high guard est utilisé")]
    [SerializeField] private Stun highGuardStun;
    [Tooltip("Stun appliqué lorsque devastating lunge est utilisé")]
    [SerializeField] private Stun devastatingLungeStun;
    [Tooltip("Stun appliqué lorsque provoke est utilisé")]
    [SerializeField] private Stun provokeStun;

    private LivingEntity livingEntity;

    private void Awake()
    {
      InjectDependencies("InjectGuardianSpells");
    }

    private void InjectGuardianSpells([GameObjectScope] LivingEntity livingEntity)
    {
      this.livingEntity = livingEntity;
    }

    /// <summary>
    /// Bash
    /// </summary>
    public void SpellX(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      bashStun.ApplyOn(livingEntity);
      GameObject bashClone = Instantiate(bash, playerPosition, Quaternion.identity);
      bashClone.GetComponentInChildren<BashController>().SetBashParameters(statsSnapshot, playerDirection, transform.root);
    }

    /// <summary>
    /// High Guard
    /// </summary>
    public void SpellY(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      highGuardStun.ApplyOn(livingEntity);
      Instantiate(highGuard, playerPosition, Quaternion.identity);
    }

    /// <summary>
    /// Devastating Lunge
    /// </summary>
    public void SpellA(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      devastatingLungeStun.ApplyOn(livingEntity);
      GameObject devastatingLungeClone = Instantiate(devastatingLunge, playerPosition, Quaternion.identity);
      devastatingLungeClone.GetComponentInChildren<DevastatingLungeController>()
        .SetDevastatingLungeParameters(statsSnapshot, transform.root, playerDirection,livingEntity);
    }

    /// <summary>
    /// Provoke
    /// </summary>
    public void SpellB(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      provokeStun.ApplyOn(livingEntity);
      GameObject provokeClone = Instantiate(provoke, playerPosition, Quaternion.identity);
      provokeClone.GetComponentInChildren<ProvokeController>().SetProvokeParameters(statsSnapshot, playerDirection);
    }
  }
}
