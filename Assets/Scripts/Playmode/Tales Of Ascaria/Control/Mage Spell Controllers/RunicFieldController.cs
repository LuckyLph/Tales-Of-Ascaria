using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
  public class RunicFieldController : GameScript
  {
    [Tooltip("Liste des effets du runic field")]
    [SerializeField] List<Effect> runicFieldEffects = new List<Effect>();

    [Tooltip("Temps avant que le runic field disparaisse")]
    [SerializeField] private float fieldDuration;

    [Tooltip("Temps entre chaque cycle d'effet du field")]
    [SerializeField] private float fieldCycleTime;

    private HitStimulus hitStimulus;

    private void InjectRunicBoltController([SiblingsScope] HitStimulus hitStimulus) 
    {
      this.hitStimulus = hitStimulus;
    }

    private void Awake()
    {
      InjectDependencies("InjectRunicBoltController");
    }

    private void OnEnable()
    {
      StartCoroutine(DoRunicField());
    }

    public IEnumerator DoRunicField()
    {
      ActorAction actorAction = new ActorAction(runicFieldEffects);
      hitStimulus.ActorAction = actorAction;
      yield return new WaitForSeconds(fieldDuration);
      Destroy(transform.root.gameObject);
    }
  }
}