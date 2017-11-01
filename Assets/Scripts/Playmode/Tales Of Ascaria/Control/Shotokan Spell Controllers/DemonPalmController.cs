using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class DemonPalmController : GameScript
  {
    [Tooltip("Effects that the Demon Palm should apply")]
    [SerializeField]
    private List<Effect> effects;

    [Tooltip("Percentage of strength to use")]
    [SerializeField]
    private float strengthPercentage;

    private Collider2D hitbox;

    private void InjectDemonPalmController([EntityScope] Collider2D hitbox)
    {
      this.hitbox = hitbox;
    }

    private void Awake()
    {
      InjectDependencies("InjectDemonPalmController");
      hitbox.enabled = false;
    }

    private void Start()
    {
      StartCoroutine(DoDemonPalm());
    }

    public void SetDemonPalmParameters(StatsSnapshot playerStats, LivingEntity player, Vector3 direction, GameObject demonPalmClone)
    {
      InstantDamage instantDamage = null;

      for (int i = 0; i < effects.Count; i++)
      {
        if (effects[i].GetType() == typeof(InstantDamage))
        {
          instantDamage = effects[i] as InstantDamage;
          instantDamage.BonusDamage = playerStats.Strength * strengthPercentage;
          effects[i] = instantDamage;
        }
        else if (effects[i].GetType() == typeof(Lifesteal))
        {
          Lifesteal lifesteal = effects[i] as Lifesteal;
          lifesteal.CastingEntity = player;
          lifesteal.DamageDealt = instantDamage.BonusDamage;
          effects[i] = lifesteal;
        }
        else if (effects[i].GetType() == typeof(Knockback))
        {
          Knockback knockback = effects[i] as Knockback;
          knockback.KnockbackSourceDirection = player.transform;
          effects[i] = knockback;
        }
      }

      //Set rotation
      demonPalmClone.transform.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(direction, Vector3.right, Vector3.back));
      if (direction.x < 0)
      {
        demonPalmClone.transform.localScale.Scale(new Vector3(-1, 0, 0));
      }

      ActorAction action = new ActorAction(effects);
      demonPalmClone.GetComponentInChildren<HitStimulus>().ActorAction = action;
    }

    private IEnumerator DoDemonPalm()
    {
      Debug.Log("Starting coroutine!");
      hitbox.enabled = true;
      yield return new WaitForSeconds(Time.deltaTime * 2);
      hitbox.enabled = false;
      yield return new WaitForSeconds(Time.deltaTime * 5);
      Destroy(gameObject.transform.root.gameObject);
    }
  }
}

