using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Contient les variable nécéssaires pour déterminer les CC affligeant une entité
  /// </summary>
  public class CrowdControl : GameScript
  {
    /// <summary>
    /// Le nombre de stun affligeant l'entité
    /// </summary>
    public int StunCounter { get; private set; }

    /// <summary>
    /// Le nombre de snare affligeant l'entité
    /// </summary>
    public int SnareCounter { get; private set; }

    /// <summary>
    /// Le pourcentage (entre 0 et 1, peut augmenter à infini) de multiplicateur de movement speed
    /// </summary>
    public float SpeedPercentage { get; private set; }

    /// <summary>
    /// Incrémente le compteurs de snares
    /// </summary>
    public void IncreaseSnareCount()
    {
      SnareCounter += 1;
    }

    /// <summary>
    /// Incrémente le compteurs de stuns
    /// </summary>
    public void IncreaseStunCount()
    {
      StunCounter += 1;
    }

    /// <summary>
    /// Réduit le compteurs de snares
    /// </summary>
    public void ReduceSnareCount()
    {
      if (SnareCounter > 0)
      {
        SnareCounter -= 1;
      }
    }

    /// <summary>
    /// Réduit le compteurs de snares
    /// </summary>
    public void ReduceStunCount()
    {
      if (StunCounter > 0)
      {
        StunCounter -= 1;
      }
    }

    /// <summary>
    /// Modifie la movespeed. Ne tombera pas en dessous de 0.
    /// </summary>
    /// <param name="speedModification">La modification. Peut être négatif</param>
    public void ModifySpeed(float speedModification)
    {
      SpeedPercentage = Mathf.Max(0f, SpeedPercentage + speedModification);
    }

    /// <summary>
    /// Remet la speed à 1.
    /// </summary>
    /// <param name="onlyResetIfSlowed">Si vrai, cela empechera le joueur de perdre un buff de movement speed</param>
    public void ResetSpeed(bool onlyResetIfSlowed = false)
    {
      if (onlyResetIfSlowed)
      {
        SpeedPercentage = Mathf.Max(1f, SpeedPercentage);
      }
      else
      {
        SpeedPercentage = 1f;
      }
    }

    public void ResetCrowdControlCount()
    {
      StunCounter = 0;
      SnareCounter = 0;
    }
  }
}
