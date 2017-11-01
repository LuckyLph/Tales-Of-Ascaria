using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class PriestSpells : GameScript, ISpellClass
  {
    [Tooltip("The Holy Blast's prefab")]
    [SerializeField]
    private GameObject holyBlast;

    [Tooltip("The Divine Decree's buff's prefab")]
    [SerializeField]
    private GameObject divineDecreeBuff;

    [Tooltip("The Divine Decree's debuff's prefab")]
    [SerializeField]
    private GameObject divineDecreeDebuff;

    [Tooltip("The Kindle Life's prefab")]
    [SerializeField]
    private GameObject kindleLife;

    [Tooltip("The Bless' prefab")]
    [SerializeField]
    private GameObject bless;

    [Tooltip("The Holy Blast's cast stun")]
    [SerializeField]
    private Stun holyBlastCastStun;

    [Tooltip("The Divine Decree's cast stun")]
    [SerializeField]
    private Stun divineDecreeCastStun;

    [Tooltip("The Kindle Life's cast stun")]
    [SerializeField]
    private Stun kindleLifeCastStun;

    [Tooltip("The Kindle Life's cast stun")]
    [SerializeField]
    private Stun blessCastStun;



    private LivingEntity entity;

    private void InjectPriestSpells([EntityScope] LivingEntity entity)
    {
      this.entity = entity;
    }


    private void Awake()
    {
      InjectDependencies("InjectPriestSpells");
    }

    public void SpellA(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      GameObject holyBlastClone = Instantiate(holyBlast, playerPosition + playerDirection / 5, Quaternion.identity);
      holyBlastClone.GetComponentInChildren<HolyBlastController>().SetHolyBlastParameters(holyBlastClone, playerDirection, statsSnapshot);
      holyBlastCastStun.ApplyOn(entity);
    }


    public void SpellB(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      GameObject kindleLifeClone = Instantiate(kindleLife, playerPosition, Quaternion.identity);
      kindleLifeClone.GetComponentInChildren<KindleLifeController>().SetKindleLifeParameters(statsSnapshot, kindleLifeClone);
      kindleLifeCastStun.ApplyOn(entity);
    }


    public void SpellX(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      GameObject divineDecreeBuffClone = Instantiate(divineDecreeBuff, playerPosition, Quaternion.identity);
      divineDecreeBuffClone.GetComponentInChildren<DivineDecreeController>().SetDivineDecreeParameters(statsSnapshot,
                                                                                                       entity,
                                                                                                       playerDirection,
                                                                                                       divineDecreeBuffClone);
      GameObject divineDecreeDebuffClone = Instantiate(divineDecreeDebuff, playerPosition, Quaternion.identity);
      divineDecreeDebuffClone.GetComponentInChildren<DivineDecreeController>().SetDivineDecreeParameters(statsSnapshot,
                                                                                                         entity,
                                                                                                         playerDirection,
                                                                                                         divineDecreeDebuffClone);
      divineDecreeCastStun.ApplyOn(entity);
    }


    public void SpellY(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      GameObject blessClone = Instantiate(bless, playerPosition, Quaternion.identity);
      blessClone.GetComponentInChildren<BlessController>().SetBlessParameters(statsSnapshot, blessClone);
      blessCastStun.ApplyOn(entity);
    }
    //
  }
}