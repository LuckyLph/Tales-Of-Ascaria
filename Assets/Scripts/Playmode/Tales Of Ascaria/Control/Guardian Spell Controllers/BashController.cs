using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class BashController : GameScript
  {
    [Tooltip("Liste des effets du bash")]
    [SerializeField]
    List<Effect> bashEffects = new List<Effect>();

    [Tooltip("Time before the attack's hitbox appears")]
    [SerializeField]
    private float attackChargeTime;

    [Tooltip("Pourcentage de force de l'entité appliqué au bash pour les dommages")]
    [SerializeField]
    private float bashPower;

    private HitStimulus hitStimulus;
    private new Collider2D collider2D;

    private void InjectBashController([EntityScope] PolygonCollider2D polygonCollider2D,
                                      [EntityScope] HitStimulus hitStimulus)
    {
      collider2D = polygonCollider2D;
      this.hitStimulus = hitStimulus;
    }

    private void Awake()
    {
      InjectDependencies("InjectBashController");
    }

    private void Start()
    {
      StartCoroutine(DoBash());
    }

    public IEnumerator DoBash()
    {
      yield return new WaitForSeconds(attackChargeTime);
      collider2D.enabled = true;
      yield return new WaitForSeconds(attackChargeTime / 2);
      Destroy(gameObject.transform.root.gameObject);
    }

    public void SetBashParameters(StatsSnapshot playerStats, Vector2 direction,
                                  Transform playerTransform)
    {
      for (int i = 0; i < bashEffects.Count; i++)
      {
        if (bashEffects[i] is InstantDamage)
        {
          InstantDamage bashInstantDamage = bashEffects[i] as InstantDamage;
          bashInstantDamage.BonusDamage = playerStats.Strength * bashPower;
          bashEffects[i] = bashInstantDamage;
        }
        if (bashEffects[i] is Knockback)
        {
          Knockback knockback = bashEffects[i] as Knockback;
          knockback.KnockbackSourceDirection = playerTransform;
          bashEffects[i] = knockback;
        }
      }
      transform.root.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(direction, Vector3.right, Vector3.back));
      ActorAction actorAction = new ActorAction(bashEffects);
      hitStimulus.ActorAction = actorAction;
    }
  }
}