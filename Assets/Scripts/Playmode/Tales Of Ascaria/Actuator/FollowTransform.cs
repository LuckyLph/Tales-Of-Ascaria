using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  /// <summary>
  /// Fait suivre un transform en temps réel.
  /// </summary>
  public class FollowTransform : GameScript
  {
    /// <summary>
    /// La cible à suivre. Le script ne fera rien si ce dernier est nul.
    /// </summary>
    public Transform target;

    private void Update()
    {
      if(target != null) transform.position = target.position;
    }
  }
}