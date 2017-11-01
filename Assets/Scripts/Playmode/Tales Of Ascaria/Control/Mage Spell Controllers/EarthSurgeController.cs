using System.Collections.Generic;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class EarthSurgeController : GameScript
  {
    [Tooltip("Liste des effets du earth surge")]
    [SerializeField] List<Effect> earthSurgeEffects = new List<Effect>();

    [Tooltip("Son joué lorsque le mage cast earth surge")]
    [SerializeField] private AudioClip EarthSurgeCastSound;

    [Tooltip("Pourcentage de wisdom de l'entité appliqué au earth surge pour les dommages")]
    [SerializeField] private float earthSurgePower;

    private HitStimulus hitStimulus;
    private new Collider2D collider2D;
    private SpriteRenderer spriteRenderer;

    private void InjectRunicBoltController([SiblingsScope] Collider2D circleCollider2D,
      [SiblingsScope] HitStimulus hitStimulus, [SiblingsScope] SpriteRenderer spriteRenderer)
    {
      collider2D = circleCollider2D;
      this.spriteRenderer = spriteRenderer;
      this.hitStimulus = hitStimulus;
    }

    private void Awake()
    {
      InjectDependencies("InjectRunicBoltController");
    }

    public void SetEarthSurgeParameters(StatsSnapshot playerStats, Vector3 direction)
    {
      for (int i = 0; i < earthSurgeEffects.Count; i++)
      {
        if (earthSurgeEffects[i] is InstantDamage)
        {
          InstantDamage earthSurgeInstantDamage = earthSurgeEffects[i] as InstantDamage;
          earthSurgeInstantDamage.BonusDamage = playerStats.Wisdom * earthSurgePower;
          earthSurgeEffects[i] = earthSurgeInstantDamage;
        }
      }
      transform.root.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(direction, Vector3.right, Vector3.back));
      ActorAction actorAction = new ActorAction(earthSurgeEffects);
      hitStimulus.ActorAction = actorAction;
    }
  }
}
