using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class HighGuardController : GameScript
  {
    [Tooltip("Liste des effets du high guard")]
    [SerializeField] List<Effect> highGuardEffects = new List<Effect>();

    [Tooltip("Temps avant que le cercle d'application d'altération commence à exister")]
    [SerializeField] private float spellChargeTime;

    [Tooltip("Temps avant que le cercle d'application d'altération cesse d'exister")]
    [SerializeField] private float buffApplicationPeriod;

    private HitStimulus hitStimulus;
    private Collider2D hitBox;

    private void Awake()
    {
      InjectDependencies("InjectHighGuardController");
    }

    private void Start()
    {
      StartCoroutine(DoHighGuard());
    }

    private void InjectHighGuardController([EntityScope] HitStimulus hitStimulus, [EntityScope] CircleCollider2D hitBox)
    {
      this.hitStimulus = hitStimulus;
      this.hitBox = hitBox;
    }

    public IEnumerator DoHighGuard()
    {
      ActorAction actorAction = new ActorAction(highGuardEffects);
      hitStimulus.GetComponentInChildren<HitStimulus>().ActorAction = actorAction;
      yield return new WaitForSeconds(spellChargeTime);
      hitBox.enabled = true;
      yield return new WaitForSeconds(buffApplicationPeriod);
      Destroy(gameObject.transform.root.gameObject);
    }
  }
}
