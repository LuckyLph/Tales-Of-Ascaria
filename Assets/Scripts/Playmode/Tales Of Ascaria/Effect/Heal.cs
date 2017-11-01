using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Heal Effect", menuName = "Game/Effect/Heal")]
  class Heal : Effect
  {
    [Tooltip("Flat amount of healing to do on target.")]
    [SerializeField]
    private float baseHealPoints;

    public float BonusHealPoints { get; set; }


    public override void ApplyOn(LivingEntity entity)
    {
     entity.Heal(baseHealPoints + BonusHealPoints);
    }
  }
}
