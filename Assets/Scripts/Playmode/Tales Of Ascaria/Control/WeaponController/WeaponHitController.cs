using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class WeaponHitController : GameScript
  {
    [Tooltip("Les effets de cet attaque. Modifiera: InstantDamage, Knockback")] [SerializeField]
    private List<Effect> weaponEffects = new List<Effect>();

    [Tooltip("Le Moveable qui sera appellé lorsque l'arme pourra attaquer. Peut être null")] [SerializeField]
    private Moveable moveable;

    private float weaponWisdomScale;
    private float weaponStrengthScale;

    private new Collider2D collider2D;

    private void Awake()
    {
      InjectDependencies("InjectWeaponHitController");
      ExecuteAttack();
    }

    private void InjectWeaponHitController([EntityScope] Collider2D collider2D)
    {
      this.collider2D = collider2D;
    }

    public void SetParameters(StatsSnapshot snapshot, Weapon weapon)
    {
      weaponStrengthScale = weapon.StrengthMultiplier;
      weaponWisdomScale = weapon.WisdomMultiplier;
      foreach (Effect effect in weaponEffects)
      {
        InstantDamage instantDamage = effect as InstantDamage;
        if (instantDamage != null)
        {
          instantDamage.BonusDamage = weapon.BaseDamage + (snapshot.Strength * weaponStrengthScale) +
                                      (snapshot.Wisdom * weaponWisdomScale);
          continue;
        }
        Knockback knockback = effect as Knockback;
        if (knockback != null)
        {
          knockback.KnockbackSourceDirection = transform.root;
        }
      }
      ActorAction actorAction = new ActorAction(weaponEffects);
      transform.root.GetComponentInChildren<HitStimulus>().ActorAction = actorAction;
    }

    public void ExecuteAttack()
    {
      StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
      if (moveable != null)
      {
        moveable.OnMoveEnd += DeleteObject;
        moveable.ExecuteMove();
      }
      yield return null;
    }

    private void DeleteObject()
    {
      if (moveable != null)
      {
        moveable.OnMoveEnd -= DeleteObject;
      }
      Destroy(transform.root.gameObject);
    }
  }
}