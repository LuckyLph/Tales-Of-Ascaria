using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class VaultController : GameScript 
	{
		[Tooltip("Effects to apply to the person hit by Vault")]
    [SerializeField]
    private List<Effect> effects;

    [Tooltip("The knockback to apply on the player.")]
    [SerializeField]
    private Knockback playerKnockback;

    [Tooltip("The strength percentage to use for bonus damage")]
    [SerializeField]
    private float strengthPercentage;

    [Tooltip("The duration of the hitbox.")]
    [SerializeField]
    private float duration;

	  private LivingEntity playerEntity;
	  private Collider2D hitbox;

		private void Awake()
		{
      Destroy(transform.parent.gameObject, duration);
      InjectDependencies("InjectVaultController");
		}


	  private void InjectVaultController([SiblingsScope] Collider2D hitbox,
                                       [EntityScope] HitStimulus hitStimulus)
	  {
	    this.hitbox = hitbox;
	    hitStimulus.OnHit += OnHit;
	  }


	  public void SetVaultParameters(Transform playerPosition, StatsSnapshot playerStats, LivingEntity playerEntity, GameObject vaultClone)
	  {
	    this.playerEntity = playerEntity;
	    for (int i = 0; i < effects.Count; i++)
	    {
	      if (effects[i] is Knockback)
	      {
	        Knockback knockback = effects[i] as Knockback;
          Debug.Log("Source direction set!");
	        knockback.KnockbackSourceDirection = playerPosition;
	        effects[i] = knockback;
	      }
        else if (effects[i] is InstantDamage)
	      {
	        InstantDamage damage = effects[i] as InstantDamage;
	        damage.BonusDamage = playerStats.Strength*strengthPercentage;
	      }
	    }

      ActorAction action = new ActorAction(effects);
	    vaultClone.GetComponentInChildren<HitStimulus>().ActorAction = action;
	  }


	  private void OnHit(ActorAction actoraction)
	  {
	    if (playerEntity != null)
	    {
        playerEntity.GetComponent<RangerSpells>().StopVaultDash();
	      playerKnockback.KnockbackSourceDirection = hitbox.transform;
        playerKnockback.ApplyOn(playerEntity);
      }
      Destroy(transform.parent.gameObject);
	  }
	}
}