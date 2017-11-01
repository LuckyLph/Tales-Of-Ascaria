using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Représente un objectif d'intéraction.
  /// </summary>
  public class InteractionObjective : Objective
  {
    public enum InteractionObjectives
    {
      Chef,
    }

    public InteractionObjectives TargetInteractorIndex
    {
      get { return targetInteractorIndex; }
      private set { targetInteractorIndex = value; }
    }

    [SerializeField]
    private InteractionObjectives targetInteractorIndex;

    public void InteractionTriggered(InteractionObjectives interactorIndex)
    {
      if (interactorIndex == TargetInteractorIndex)
      {
        InvokeOnObjectiveCompleted(this);
      }
    }
  }
}