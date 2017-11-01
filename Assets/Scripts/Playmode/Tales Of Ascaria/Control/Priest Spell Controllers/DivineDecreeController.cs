using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class DivineDecreeController : GameScript
  {
    [Tooltip("The amount of wisdom to use for the damage to mobs, if any.")]
    [SerializeField]
    private float wisdomDamagePercentage;

    [Tooltip("The power factor for the stats modification. Anything in or above 1 is a buff. Anything under 1 is a nerf.")]
    [SerializeField]
    private float powerFactor;

    [Tooltip("The effects to apply on this instance of Divine Decree")]
    [SerializeField]
    private List<Effect> effects;

    private Collider2D hitbox;


    private void InjectDivineDecreeController([EntityScope] Collider2D hitbox)
    {
      this.hitbox = hitbox;
    }


    private void Awake()
    {
      InjectDependencies("InjectDivineDecreeController");
    }


    private void Start()
    {
      hitbox.enabled = false;
      StartCoroutine(DoDivineDecree());
    }


    public void SetDivineDecreeParameters(StatsSnapshot playerStats, LivingEntity player, Vector3 direction, GameObject divineDecreeClone)
    {
      float finalModifier;
      if (powerFactor >= 1)
      {
        finalModifier = 1 - (100/(100 + (playerStats.Wisdom)));
        Debug.Log("Positive Final modifier : " + finalModifier);
      }
      else
      {
        finalModifier = 1 - ((1 - (100/(100 + playerStats.Wisdom/2))) * (powerFactor*2));
        Debug.Log("Negative Final modifier : " + finalModifier);
      }
      for (int i = 0; i < effects.Count; i++)
      {
        if (effects[i] is StatsModifier)
        {
          StatsModifier modifier = effects[i] as StatsModifier;
          modifier.SpiritModifier = finalModifier;
          modifier.ConstitutionModifier = finalModifier;
          effects[i] = modifier;
        }
        else if (effects[i] is InstantDamage)
        {
          InstantDamage damage = effects[i] as InstantDamage;
          damage.BonusDamage = playerStats.Wisdom*wisdomDamagePercentage;
          effects[i] = damage;
        }
      }

      //Set rotation
      divineDecreeClone.transform.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(direction, Vector3.right, Vector3.back));

      ActorAction action = new ActorAction(effects);
      divineDecreeClone.GetComponentInChildren<HitStimulus>().ActorAction = action;
    }


    private IEnumerator DoDivineDecree()
    {
      hitbox.enabled = true;
      yield return new WaitForSeconds(Time.deltaTime * 5);
      hitbox.enabled = false;
      yield return new WaitForSeconds(Time.deltaTime * 5);
      Destroy(transform.root.gameObject);
    }
  }
}

