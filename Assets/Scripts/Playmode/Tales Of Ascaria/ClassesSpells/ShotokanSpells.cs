using System.Collections;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ShotokanSpells : GameScript, ISpellClass
  {
    [Tooltip("The Ki Blast's prefab")]
    [SerializeField]
    private GameObject kiBlast;

    [Tooltip("The Zen Hurricane's prefab")]
    [SerializeField]
    private GameObject zenHurricane;

    [Tooltip("The Demon Palm's prefab")]
    [SerializeField]
    private GameObject demonPalm;

    [Tooltip("The focus attack strike's prefab")]
    [SerializeField]
    private GameObject focusAttackStrike;

    [Tooltip("The focus attack impact's prefab")]
    [SerializeField]
    private GameObject focusAttackImpact;

    [Tooltip("The focus attack cancel's knockback (as a dash)")]
    [SerializeField]
    private Dash focusAttackCancelDash;

    [Tooltip("The damage reduction while the player is in focus attack")]
    [SerializeField]
    private StatsModifier focusAttackDamageReduction;

    [Tooltip("Stun to be applied upon casting Ki Blast")]
    [SerializeField]
    private Stun kiBlastCastStun;

    [Tooltip("Stun to be applied upon casting Zen Hurricane")]
    [SerializeField]
    private Stun zenHurricaneCastStun;

    [Tooltip("Stun to be applied upon casting Demon Palm")]
    [SerializeField]
    private Stun demonPalmCastStun;

    [Tooltip("Stun to be applied upon casting Focus Attack : Strike")]
    [SerializeField]
    private Stun focusAttackStrikeCastStun;

    [Tooltip("Stun to be applied upon casting Focus Attack : Impact")]
    [SerializeField]
    private Stun focusAttackImpactCastStun;

    [Tooltip("Stun to be applied upon casting Focus Attack : Cancel")]
    [SerializeField]
    private Stun focusAttackDashCastStun;

    [Tooltip("Maximum duration of the Focus Attack in seconds")]
    [SerializeField]
    private float focusAttackDuration;

    private bool isInFocusAttack = false;
    private LivingEntity playerEntity;

    private void InjectShotokanSpells([EntityScope] LivingEntity livingEntity)
    {
      this.playerEntity = livingEntity;
    }


    private void Awake()
    {
      InjectDependencies("InjectShotokanSpells");
    }


    public void SpellX(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (isInFocusAttack)
      {
        isInFocusAttack = false;
        GameObject focusAttackStrikeClone = Instantiate(focusAttackStrike, playerPosition + playerDirection / 5, Quaternion.identity);
        focusAttackStrikeClone.GetComponentInChildren<FocusAttackStrikeController>().SetFocusAttackStrikeParameters(focusAttackStrikeClone,
                                                                                                                    playerDirection,
                                                                                                                    statsSnapshot,
                                                                                                                    transform.root.transform);
        focusAttackStrikeCastStun.ApplyOn(playerEntity);
      }
      else
      {
        GameObject demonPalmClone = Instantiate(demonPalm, playerPosition + playerDirection / 5, Quaternion.identity);
        demonPalmClone.GetComponentInChildren<DemonPalmController>().SetDemonPalmParameters(statsSnapshot,
                                                                                            playerEntity,
                                                                                            playerDirection,
                                                                                            demonPalmClone);
        demonPalmCastStun.ApplyOn(playerEntity);
      }
    }

    public void SpellY(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (!isInFocusAttack)
      {
        isInFocusAttack = true;
        playerEntity.StartCoroutine(ApplyFocusAttackResistance());
      }
      else
      {
        isInFocusAttack = false;
      }
    }

    public void SpellA(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (isInFocusAttack)
      {
        isInFocusAttack = false;
        GameObject focusAttackImpactClone = Instantiate(focusAttackImpact, playerPosition, Quaternion.identity);
        focusAttackImpactClone.GetComponentInChildren<FocusAttackImpactController>().SetFocusAttackImpactParameters(focusAttackImpact,
                                                                                                                    playerDirection,
                                                                                                                    statsSnapshot);
        focusAttackImpactCastStun.ApplyOn(playerEntity);
      }
      else
      {
        GameObject kiBlastClone = Instantiate(kiBlast, playerPosition + playerDirection / 5, Quaternion.identity);
        kiBlastClone.GetComponentInChildren<KiBlastController>().SetKiBlastParameters(kiBlastClone, playerDirection, statsSnapshot);
        kiBlastCastStun.ApplyOn(playerEntity);
      }
    }

    public void SpellB(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      if (isInFocusAttack)
      {
        focusAttackCancelDash.DashDirection = playerDirection;
        focusAttackCancelDash.ApplyOn(playerEntity);
        isInFocusAttack = false;
      }
      else
      {
        GameObject zenHurricaneClone = Instantiate(zenHurricane, playerPosition, Quaternion.identity);
        zenHurricaneClone.GetComponentInChildren<ZenHurricaneController>()
                         .SetZenHurricaneParameters(zenHurricaneClone, statsSnapshot, transform.root.transform);
        zenHurricaneCastStun.ApplyOn(playerEntity);
      }
    }


    private IEnumerator ApplyFocusAttackResistance()
    {
      float endtime = Time.time + focusAttackDuration;
      playerEntity.GetCrowdControl().IncreaseSnareCount();
      focusAttackDamageReduction.ApplyOn(playerEntity);
      while (isInFocusAttack)
      {
        if (endtime < Time.time)
        {
          isInFocusAttack = false;
        }
        yield return new WaitForEndOfFrame();
      }
      focusAttackDamageReduction.Cleanse(playerEntity);
      playerEntity.GetCrowdControl().ReduceSnareCount();
      yield return null;
    }
  }
}