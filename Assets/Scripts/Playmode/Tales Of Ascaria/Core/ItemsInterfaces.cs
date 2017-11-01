using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Une classe qui hérite de cette interface peut être déposée au sol.
  /// </summary>
  public interface IDroppable
  {
    /// <summary>
    /// Dépose l'item au sol
    /// </summary>
    /// <param name="position">L'endroit ou le déposer</param>
    void Drop(Vector3 position);
  }

  /// <summary>
  /// Une classe qui hérite de cette interface peut être consommée
  /// </summary>
  public interface IConsummable
  {
    /// <summary>
    /// Utilise l'item
    /// </summary>
    /// <param name="livingEntity">Le living entity pour appliquer tout effet</param>
    void Use(LivingEntity livingEntity, Effect[] effect);
  }
}