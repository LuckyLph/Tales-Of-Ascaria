using System.Collections;
using UnityEngine;
using Harmony;
using System.Collections.Generic;

namespace TalesOfAscaria 
{
	public class BlessController : GameScript 
	{
    [Tooltip("The list of effects to apply upon using the spell.")]
    [SerializeField]
    private List<Effect> effects;

	  private float duration;
	  private Collider2D hitbox;

	  private void InjectBlessController([EntityScope] Collider2D hitbox)
	  {
	    this.hitbox = hitbox;
	  }


	  private void Awake()
	  {
      InjectDependencies("InjectBlessController");
	    hitbox.enabled = false;
	  }


	  private void Start()
	  {
	    StartCoroutine(DoBless());
	  }


	  public void SetBlessParameters(StatsSnapshot playerStats, GameObject blessClone)
	  {
	    for (int i = 0; i < effects.Count; i++)
	    {
	      if (effects[i] is StatsModifier)
	      {
	        StatsModifier modifier = effects[i] as StatsModifier;
          modifier.StrengthModifier = 1 - (100 / (100 + (playerStats.Wisdom)));
          modifier.WisdomModifier = 1 - (100 / (100 + (playerStats.Wisdom)));
	        effects[i] = modifier;
	      }
	    }

      ActorAction action = new ActorAction(effects);
      blessClone.GetComponentInChildren<HitStimulus>().ActorAction = action;
    }


	  private IEnumerator DoBless()
	  {
	    hitbox.enabled = true;
      yield return new WaitForSeconds(Time.deltaTime * 2);
	    hitbox.enabled = false;
      Destroy(transform.root.gameObject);
	  }
	}
}