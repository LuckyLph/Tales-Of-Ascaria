using System.Collections.Generic;
using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void QuestCompletedHandler(Quest quest);

  public delegate void ObjectiveStartedHandler(Objective newObjective);


  /// <summary>
  /// Classe qui représente une quête. Gère les objectifs en cours, reçoit les événements des objectifs qu'elle contient et notifie le controlleur lorsqu'elle ou un de des objectifs sont complété.
  /// </summary>
  public class Quest : GameScript
  {
    #region Variables

    public event QuestCompletedHandler OnQuestCompleted;
    public event ObjectiveStartedHandler OnObjectiveStarted;
    public event ObjectiveCompletedHandler OnObjectiveCompleted;

    public Objective ActiveObjective
    {
      get { return activeObjective; }
      set { activeObjective = value; }
    }

    public List<Objective> CurrentObjectives
    {
      get { return currentObjectives; }
      private set { currentObjectives = value; }
    }

    public Objective[] Objectives
    {
      get { return objectives; }
      private set { objectives = value; }
    }

    public int[] NextQuestsIndexes
    {
      get { return nextQuestsIndexes; }
      private set { nextQuestsIndexes = value; }
    }

    public int GoldReward
    {
      get { return goldReward; }
      private set { goldReward = value; }
    }

    public int XpReward
    {
      get { return xpReward; }
      private set { xpReward = value; }
    }

    public string QuestName
    {
      get { return questName; }
      private set { questName = value; }
    }

    public bool IsCompleted
    {
      get { return isCompleted; }
      private set { isCompleted = value; }
    }

    [SerializeField]
    private List<Objective> currentObjectives;

    [SerializeField]
    private Objective[] objectives;

    [SerializeField]
    private Objective activeObjective;

    [SerializeField]
    private int[] nextQuestsIndexes;

    [SerializeField]
    private int goldReward;

    [SerializeField]
    private int xpReward;

    [SerializeField]
    private string questName;

    [SerializeField]
    private bool isCompleted;

    #endregion

    #region Initialization

    private void Awake()
    {
      for (int i = 0; i < objectives.Length; i++)
      {
        objectives[i].OnObjectiveCompleted += ObjectiveCompleted;
      }
    }

    private void OnDestroy()
    {
      for (int i = 0; i < objectives.Length; i++)
      {
        objectives[i].OnObjectiveCompleted -= ObjectiveCompleted;
      }
    }

    public void QuestStarted()
    {
      currentObjectives.Add(objectives[0]);
      activeObjective = currentObjectives[0];
    }

    #endregion

    #region Events Channels

    public void InteractionTriggered(InteractionObjective.InteractionObjectives interactorIndex)
    {
      for (int i = 0; i < currentObjectives.Count; i++)
      {
        if (currentObjectives[i] is InteractionObjective)
        {
          InteractionObjective interactionObjective = currentObjectives[i] as InteractionObjective;
          interactionObjective.InteractionTriggered(interactorIndex);
        }
      }
    }

    public void ExplorationTriggered(ExplorationObjective.ExplorationObjectives explorationNodeIndex)
    {
      for (int i = 0; i < currentObjectives.Count; i++)
      {
        if (currentObjectives[i] is ExplorationObjective)
        {
          ExplorationObjective explorationObjective = currentObjectives[i] as ExplorationObjective;
          explorationObjective.ExplorationTriggered(explorationNodeIndex);
        }
      }
    }

    public void EnemyKilled()
    {
    }

    public void ItemAcquired()
    {
    }

    #endregion

    #region Objective Events

    private void ObjectiveCompleted(Objective completedObjective)
    {
      for (int i = 0; i < objectives.Length; i++)
      {
        for (int j = 0; j < completedObjective.NextObjectivesIndexes.Length; j++)
        {
          if (objectives[i] == objectives[completedObjective.NextObjectivesIndexes[j]])
          {
            currentObjectives.Add(objectives[i]);
            if (OnObjectiveStarted != null) OnObjectiveStarted(objectives[i]);
          }
        }
      }
      currentObjectives.Remove(completedObjective);


      if (activeObjective == completedObjective && currentObjectives.Count > 0)
      {
        activeObjective = currentObjectives[0];
      }
      if (OnObjectiveCompleted != null)
      {
        OnObjectiveCompleted(completedObjective);
      }
      if (currentObjectives.Count == 0)
      {
        if (OnQuestCompleted != null) OnQuestCompleted(this);
        isCompleted = true;
      }
    }

    #endregion
  }
}