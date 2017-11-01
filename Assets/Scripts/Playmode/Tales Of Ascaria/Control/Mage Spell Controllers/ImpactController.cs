using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ImpactController : GameScript
  {
    [Tooltip("Liste des effets du impact")]
    [SerializeField] List<Effect> impactEffects = new List<Effect>();

    [Tooltip("Time before the attack appears")]
    [SerializeField] private float attackChargeTime;

    [Tooltip("Pourcentage de wisdom de l'entité appliqué au impact pour les dommages instantannés")]
    [SerializeField] private float impactPower;

    private HitStimulus hitStimulus;
    private new Collider2D collider2D;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
      InjectDependencies("InjectImpact");
    }

    private void Start()
    {
      StartCoroutine(DoImpact());
    }

    private void InjectImpact([SiblingsScope] HitStimulus hitStimulus, [SiblingsScope] Collider2D collider2D, 
                              [SiblingsScope] SpriteRenderer spriteRenderer)
    {
      this.collider2D = collider2D;
      this.hitStimulus = hitStimulus;
      this.spriteRenderer = spriteRenderer;
    }

    public void SetImpactParameters(StatsSnapshot statsSnapshot, Transform casterTransform)
    {
      for (int i = 0; i < impactEffects.Count; i++)
      {
        if (impactEffects[i] is InstantDamage)
        {
          InstantDamage impactDamage = impactEffects[i] as InstantDamage;
          impactDamage.BonusDamage = statsSnapshot.Wisdom * impactPower;
          impactEffects[i] = impactDamage;
        }
        else if (impactEffects[i] is Knockback)
        {
          Knockback impactKnockback = impactEffects[i] as Knockback;
          impactKnockback.KnockbackSourceDirection = casterTransform;
          impactEffects[i] = impactKnockback;
        }
      }
      ActorAction actorAction = new ActorAction(impactEffects);
      hitStimulus.ActorAction = actorAction;
    }

    public IEnumerator DoImpact()
    {
      yield return new WaitForSeconds(attackChargeTime);
      collider2D.enabled = true;
      spriteRenderer.enabled = true;
      yield return new WaitForSeconds(Time.deltaTime*2);
      Destroy(transform.root.gameObject);
    }
  }
}