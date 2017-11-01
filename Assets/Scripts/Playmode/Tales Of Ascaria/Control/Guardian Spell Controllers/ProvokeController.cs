using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ProvokeController : GameScript
  {
    [Tooltip("Liste des effets du provoke")]
    [SerializeField]
    List<Effect> provokeEffects = new List<Effect>();

    [Tooltip("Temps avant que le rayon d'application d'altération commence à exister")]
    [SerializeField]
    private float spellChargeTime;

    [Tooltip("Temps avant que le rayon d'application d'altération cesse d'exister")]
    [SerializeField]
    private float debuffApplicationPeriod;

    [Tooltip("Pourcentage de force de l'entité appliqué au provoke pour les dommages")]
    [SerializeField]
    private float provokePower;

    private HitStimulus hitStimulus;
    private new Collider2D collider2D;

    private void Awake()
    {
      InjectDependencies("InjectProvokeController");
    }

    private void Start()
    {
      StartCoroutine(DoProvoke());
    }

    private void InjectProvokeController([EntityScope] PolygonCollider2D collider2D, [EntityScope] HitStimulus hitStimulus)
    {
      this.hitStimulus = hitStimulus;
      this.collider2D = collider2D;
    }

    public IEnumerator DoProvoke()
    {
      yield return new WaitForSeconds(spellChargeTime);
      collider2D.enabled = true;
      yield return new WaitForSeconds(debuffApplicationPeriod);
      Destroy(gameObject.transform.root.gameObject);
    }

    public void SetProvokeParameters(StatsSnapshot playerStats, Vector2 playerDirection)
    {
      for (int i = 0; i < provokeEffects.Count; i++)
      {
        if (provokeEffects[i] is InstantDamage)
        {
          InstantDamage bashInstantDamage = provokeEffects[i] as InstantDamage;
          bashInstantDamage.BonusDamage = playerStats.Strength * provokePower;
          provokeEffects[i] = bashInstantDamage;
        }
      }
      transform.root.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(playerDirection, Vector3.right, Vector3.back));
      ActorAction actorAction = new ActorAction(provokeEffects);
      hitStimulus.ActorAction = actorAction;
    }
  }
}