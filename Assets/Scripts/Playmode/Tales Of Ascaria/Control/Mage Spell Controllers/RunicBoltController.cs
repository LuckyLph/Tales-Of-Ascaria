using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class RunicBoltController : GameScript
  {
    [Tooltip("Liste des effets du runic bolt")]
    [SerializeField] List<Effect> runicBoltEffects = new List<Effect>();

    [Tooltip("Time before the attack appears")]
    [SerializeField] private float attackChargeTime;

    [Tooltip("Pourcentage de wisdom de l'entité appliqué au runic bolt pour les dommages instantannés")]
    [SerializeField] private float runicBoltPower;

    [Tooltip("Field qui apparait lorsque le runic bolt est détruit")]
    [SerializeField] private GameObject runicField;

    private HitStimulus hitStimulus;
    private new Collider2D collider2D;
    private ProjectileController projectileController;
    private SpriteRenderer spriteRenderer;

    private void InjectRunicBoltController([SiblingsScope] CircleCollider2D circleCollider2D,
      [SiblingsScope] HitStimulus hitStimulus, [SiblingsScope] ProjectileController projectileController,
      [SiblingsScope] SpriteRenderer spriteRenderer)
    {
      collider2D = circleCollider2D;
      this.hitStimulus = hitStimulus;
      this.projectileController = projectileController;
      this.spriteRenderer = spriteRenderer;
    }

    private void Awake()
    {
      InjectDependencies("InjectRunicBoltController");
    }

    private void Start()
    {
      StartCoroutine(DoRunicBolt());
    }

    private void OnDestroy()
    {
      Instantiate(runicField, transform.position, Quaternion.identity);
    }

    public IEnumerator DoRunicBolt()
    {
      yield return new WaitForSeconds(attackChargeTime);
      collider2D.enabled = true;
      spriteRenderer.enabled = true;
      projectileController.enabled = true;
    }

    public void SetRunicBoltParameters(StatsSnapshot playerStats, Vector3 direction)
    {
      for (int i = 0; i < runicBoltEffects.Count; i++)
      {
        if (runicBoltEffects[i] is InstantDamage)
        {
          InstantDamage runicBoltInstantDamage = runicBoltEffects[i] as InstantDamage;
          runicBoltInstantDamage.BonusDamage = playerStats.Wisdom * runicBoltPower;
          runicBoltEffects[i] = runicBoltInstantDamage;
        }
      }
      transform.root.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(direction, Vector3.right, Vector3.back));
      projectileController.Direction = direction;
      ActorAction actorAction = new ActorAction(runicBoltEffects);
      hitStimulus.ActorAction = actorAction;
    }
  }
}