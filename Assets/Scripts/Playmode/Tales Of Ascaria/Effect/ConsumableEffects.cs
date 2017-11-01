using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New ConsumableEffects", menuName = "Game/Effect/EffectContainer/ConsumableEffects")]
  public class ConsumableEffects : ScriptableObject
  {

    [Tooltip("Les effets contenus dans la potion. Les numériques doivent être à 0")]
    [SerializeField]
    public Effect[] effects;

  }
}


