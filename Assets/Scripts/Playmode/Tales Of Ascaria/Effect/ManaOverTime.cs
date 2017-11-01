using System;
using System.Collections;
using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Représente un effet qui ajoute de la mana s'effectuant sur une période de temps
  /// </summary>
  [CreateAssetMenu(fileName = "New ManaOverTimeEffect", menuName = "Game/Effect/ManaOverTime")]
  public class ManaOverTime : Effect
  {
    [Tooltip("Le nombre de mana à ajouter au total")]
    [SerializeField]
    private int totalMana;

    [Tooltip("Le nombre de tick par seconde. N'affecte pas le mana total")]
    [SerializeField]
    private float ticksPerSecond;

    [Tooltip("La durée totale")]
    [SerializeField]
    private float duration;

    private float timeAtStart;


    public void SetMana(int healAmount)
    {
      totalMana = healAmount;
    }

    public override void ApplyOn(LivingEntity entity)
    {
      timeAtStart = Time.time;
      entity.StartCoroutine(DoManaOverTime(entity));
    }

    private IEnumerator DoManaOverTime(LivingEntity entity)
    {
      float timeBetweenTicks = (1f / ticksPerSecond);
      int totalTicks = Mathf.FloorToInt(duration / timeBetweenTicks);
      int manaPertick = Mathf.RoundToInt(totalMana / totalTicks);

      while (Time.time <= timeAtStart + duration)
      {
        //WISH: Utiliser le living entity a la place (quand tommy aura fait le pont)
        entity.GetComponent<Mana>().HealMana(manaPertick);
        Debug.Log("I healed mana for: " + manaPertick);
        yield return new WaitForSeconds(timeBetweenTicks);
      }
    }
  }
}