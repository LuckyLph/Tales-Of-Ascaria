using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Harmony;

namespace TalesOfAscaria
{
  class KindleLifeController : GameScript
  {
    [Tooltip("The percentage of wisdom to add to the base healing effect.")]
    [SerializeField]
    private float wisdomPercentage;

    [Tooltip("The effects to apply upon entering the healing hitbox.")]
    [SerializeField]
    private List<Effect> effects;

    private Collider2D hitbox;


    private void InjectKindleLifeController([EntityScope] Collider2D hitbox)
    {
      this.hitbox = hitbox;
    }


    private void Awake()
    {
      InjectDependencies("InjectKindleLifeController");
    }


    private void Start()
    {
      StartCoroutine(ApplyHeal());
    }


    public void SetKindleLifeParameters(StatsSnapshot playerStats, GameObject kindleLifeClone)
    {
      for (int i = 0; i < effects.Count; i++)
      {
        if (effects[i] is Heal)
        {
          Heal heal = effects[i] as Heal;
          heal.BonusHealPoints = playerStats.Wisdom*wisdomPercentage;
          effects[i] = heal;
        }
      }
    }


    private IEnumerator ApplyHeal()
    {
      hitbox.enabled = true;
      yield return new WaitForSeconds(Time.deltaTime * 2);
      hitbox.enabled = false;
      yield return new WaitForSeconds(0.5f);
      Destroy(gameObject.transform.root.gameObject);
    }
  }
}
