using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void ObjectiveCompletedHandler(Objective completedObjective);

  /// <summary>
  /// Représente un objectif de quête.
  /// </summary>
  public abstract class Objective : GameScript
  {
    public event ObjectiveCompletedHandler OnObjectiveCompleted;

    public int[] NextObjectivesIndexes
    {
      get { return nextObjectivesIndexes; }
      private set { nextObjectivesIndexes = value; }
    }

    public string ObjectiveDescription
    {
      get { return objectiveDescription; }
      private set { objectiveDescription = value; }
    }

    [SerializeField]
    protected int[] nextObjectivesIndexes;

    [SerializeField]
    protected string objectiveDescription;

    protected void InvokeOnObjectiveCompleted(Objective objective)
    {
      if (OnObjectiveCompleted != null) OnObjectiveCompleted(objective);
    }
  }
}