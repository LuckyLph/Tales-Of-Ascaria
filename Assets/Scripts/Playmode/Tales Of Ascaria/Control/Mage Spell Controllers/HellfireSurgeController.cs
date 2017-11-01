using System.Collections;
using System.Collections.Generic;
using Harmony;
using TalesOfAscaria;
using UnityEngine;

public class HellfireSurgeController : GameScript
{
  [Tooltip("Liste des effets du bash")]
  [SerializeField]
  List<Effect> hellfireSurgeEffects = new List<Effect>();

  [Tooltip("Time before the attack appears")]
  [SerializeField]
  private float attackChargeTime;

  [Tooltip("Pourcentage de wisdom de l'entité appliqué au hellfire surge pour les dommages instantannés")]
  [SerializeField]
  private float hellfireSurgePower;

  [Tooltip("Pourcentage de force wisdom de l'entité appliqué au hellfire surge pour le damage over time")]
  [SerializeField]
  private float hellfireSurgeTickPower;

  private HitStimulus hitStimulus;
  private new Collider2D collider2D;
  private ProjectileController projectileController;
  private SpriteRenderer spriteRenderer;

  private void InjectHellfireSurgeController([EntityScope] CircleCollider2D circleCollider2D,
    [EntityScope] HitStimulus hitStimulus, [EntityScope] ProjectileController projectileController,
    [EntityScope] SpriteRenderer spriteRenderer)
  {
    collider2D = circleCollider2D;
    this.hitStimulus = hitStimulus;
    this.projectileController = projectileController;
    this.spriteRenderer = spriteRenderer;
  }

  private void Awake()
  {
    InjectDependencies("InjectHellfireSurgeController");
  }

  private void Start()
  {
    StartCoroutine(DoHellfireSurge());
  }

  public IEnumerator DoHellfireSurge()
  {
    yield return new WaitForSeconds(attackChargeTime);
    collider2D.enabled = true;
    spriteRenderer.enabled = true;
    projectileController.enabled = true;
    yield return new WaitForSeconds(attackChargeTime);
  }

  public void SetHellfireSurgeParameters(StatsSnapshot playerStats, Vector3 direction)
  {
    for (int i = 0; i < hellfireSurgeEffects.Count; i++)
    {
      if (hellfireSurgeEffects[i] is InstantDamage)
      {
        InstantDamage hellfireSurgeInstantDamage = hellfireSurgeEffects[i] as InstantDamage;
        hellfireSurgeInstantDamage.BonusDamage = playerStats.Wisdom * hellfireSurgePower;
        hellfireSurgeEffects[i] = hellfireSurgeInstantDamage;
      }
      else if (hellfireSurgeEffects[i] is DamageOverTime)
      {
        DamageOverTime hellfireSurgeDamagePerTick = hellfireSurgeEffects[i] as DamageOverTime;
        hellfireSurgeDamagePerTick.BonusDamagePerTick = playerStats.Wisdom * hellfireSurgeTickPower;
        hellfireSurgeEffects[i] = hellfireSurgeDamagePerTick;
      }
    }
    transform.root.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(direction, Vector3.right, Vector3.back));
    projectileController.Direction = direction;
    ActorAction actorAction = new ActorAction(hellfireSurgeEffects);
    hitStimulus.ActorAction = actorAction;
  }
}
