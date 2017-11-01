using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Harmony;

namespace TalesOfAscaria 
{
	public class WeaselPrimaryAttackController : GameScript 
	{
    [Tooltip("The primary attack's effects list")]
    [SerializeField]
    List<Effect> primaryAttackEffects = new List<Effect>();

    [Tooltip("The percentage of the weasel's strength the spell uses.")]
    [SerializeField]
    private float primaryAttackPower;

    private HitStimulus hitStimulus;
    private new Collider2D collider;

    private void InjectWeaselPrimaryAttackController([SiblingsScope] HitStimulus hitStimulus,
                                                     [SiblingsScope] Collider2D collider)
    {
      this.collider = collider;
      this.hitStimulus = hitStimulus;
    }


    private void Awake() 
		{
      InjectDependencies("InjectWeaselPrimaryAttackController");
      collider.enabled = false;
		}

    private void Start()
    {
      StartCoroutine(DoPrimaryAttack());
    }

    public IEnumerator DoPrimaryAttack()
    {
      collider.enabled = true;
      yield return new WaitForSeconds(Time.deltaTime * 2);
      collider.enabled = false;
      Destroy(gameObject.transform.parent.gameObject);
    }

    public void SetWeaselPrimaryAttackControllerParameters(StatsSnapshot playerStats, Vector2 direction, GameObject attackClone)
    {
      for (int i = 0; i < primaryAttackEffects.Count; i++)
      {
        if (primaryAttackEffects[i] is InstantDamage)
        {
          InstantDamage bashInstantDamage = primaryAttackEffects[i] as InstantDamage;
          bashInstantDamage.BonusDamage = playerStats.Strength * primaryAttackPower;
          primaryAttackEffects[i] = bashInstantDamage;
        }
      }
      attackClone.transform.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(direction, Vector3.right, Vector3.back));
      ActorAction actorAction = new ActorAction(primaryAttackEffects);
      hitStimulus.ActorAction = actorAction;
    }
  }
}