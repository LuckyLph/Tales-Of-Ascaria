using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Représente un objectif d'exploration.
  /// </summary>
  public class ExplorationObjective : Objective
  {
    public enum ExplorationObjectives
    {
      Bridge,
      Quarry,
      Castle,
    }

    public ExplorationObjectives TargetExplorationNodeIndex
    {
      get { return targetExplorationNodeIndex; }
      private set { targetExplorationNodeIndex = value; }
    }

    [SerializeField]
    private ExplorationObjectives targetExplorationNodeIndex;

    public void ExplorationTriggered(ExplorationObjectives explorationNodeIndex)
    {
      if (explorationNodeIndex == TargetExplorationNodeIndex)
      {
        InvokeOnObjectiveCompleted(this);
      }
    }
  }
}