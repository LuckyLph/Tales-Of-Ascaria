using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class DeathLotusController : GameScript
  {
    [Tooltip("Effects that the DeathLotus should apply")]
    [SerializeField] List<Effect> deathLotusEffects = new List<Effect>();

    [Tooltip("Pourcentage de strength de l'entité appliqué au death lotus pour les dommages")]
    [SerializeField] private float deathLotusPower;

    private HitStimulus hitStimulus;
    private SpriteRenderer spriteRenderer;

    private Transform casterTransform;

    private void Awake()
    {
      InjectDependencies("InjectDeathLotusController");
    }

    private void Update()
    {
      transform.parent.position = casterTransform.position;
    }

    private void InjectDeathLotusController([SiblingsScope] HitStimulus hitStimulus,
                                            [SiblingsScope] SpriteRenderer spriteRenderer)
    {
      this.hitStimulus = hitStimulus;
      this.spriteRenderer = spriteRenderer;
    }

    public void SetDeathLotusParameters(StatsSnapshot playerStats, Transform casterTransform)
    {
      for (int i = 0; i < deathLotusEffects.Count; i++)
      {
        if (deathLotusEffects[i] is InstantDamage)
        {
          InstantDamage deathLotusInstantDamage = deathLotusEffects[i] as InstantDamage;
          deathLotusInstantDamage.BonusDamage = playerStats.Wisdom * deathLotusPower;
          deathLotusEffects[i] = deathLotusInstantDamage;
        }
      }
      ActorAction actorAction = new ActorAction(deathLotusEffects);
      hitStimulus.ActorAction = actorAction;
      this.casterTransform = casterTransform;
    }
  }
}
