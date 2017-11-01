using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Controlleur du système de quêtes. Gère les quêtes en cours, reçoit les événements des canaux auxquels il est abonné et met à jour le HUD.
  /// </summary>
  public class QuestManagerController : GameScript
  {
    public Quest ActiveQuest
    {
      get { return activeQuest; }
      set { activeQuest = value; }
    }

    public List<Quest> CurrentQuests
    {
      get { return currentQuests; }
      private set { currentQuests = value; }
    }

    public Quest[] Quests
    {
      get { return quests; }
      private set { quests = value; }
    }

    public Quest[] RawQuests
    {
      get { return rawQuests; }
      private set { rawQuests = value; }
    }

    [SerializeField]
    private Quest activeQuest;

    [SerializeField]
    private List<Quest> currentQuests;

    [SerializeField]
    private Quest[] quests;

    [SerializeField]
    private Quest[] rawQuests;

    private CurrentQuestView currentQuestView;
    private CurrentObjectiveView currentObjective1View;
    private CurrentObjectiveView currentObjective2View;
    private CurrentObjectiveView currentObjective3View;
    private ItemAcquiredEventChannel itemAcquiredEventChannel;
    private ExplorationEventChannel explorationEventChannel;
    private DestroyEventChannel destroyEventChannel;
    private InteractionEventChannel interactionEventChannel;
    private QuestStartedEventChannel questStartedEventChannel;
    private QuestEndedEventChannel questEndedEventChannel;

    #region Initialization

    private void InjectQuestManagerController([EventChannelScope] ItemAcquiredEventChannel itemAcquiredEventChannel,
                                             [EventChannelScope] ExplorationEventChannel explorationEventChannel,
                                             [EventChannelScope] DestroyEventChannel destroyEventChannel,
                                             [EventChannelScope] InteractionEventChannel interactionEventChannel,
                                             [EventChannelScope] QuestStartedEventChannel questStartedEventChannel,
                                             [EventChannelScope] QuestEndedEventChannel questEndedEventChannel)
    {
      this.itemAcquiredEventChannel = itemAcquiredEventChannel;
      this.explorationEventChannel = explorationEventChannel;
      this.destroyEventChannel = destroyEventChannel;
      this.interactionEventChannel = interactionEventChannel;
      this.questStartedEventChannel = questStartedEventChannel;
      this.questEndedEventChannel = questEndedEventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectQuestManagerController");

      explorationEventChannel.OnEventPublished += OnExplorationEventPublished;
      itemAcquiredEventChannel.OnEventPublished += OnItemAcquiredEventPublished;
      destroyEventChannel.OnEventPublished += OnDestroyEventPublished;
      interactionEventChannel.OnEventPublished += OnInteractionEventPublished;
      questStartedEventChannel.OnEventPublished += OnQuestStartedEventPublished;
      questEndedEventChannel.OnEventPublished += OnQuestEndedEventPublished;

      quests = new Quest[rawQuests.Length];
      for (int i = 0; i < rawQuests.Length; i++)
      {
        quests[i] = Instantiate(rawQuests[i], transform);
      }
      for (int i = 0; i < quests.Length; i++)
      {
        quests[i].OnObjectiveCompleted += OnObjectiveCompleted;
        quests[i].OnQuestCompleted += OnQuestCompleted;
      }
    }

    private void OnDestroy()
    {
      explorationEventChannel.OnEventPublished -= OnExplorationEventPublished;
      itemAcquiredEventChannel.OnEventPublished -= OnItemAcquiredEventPublished;
      destroyEventChannel.OnEventPublished -= OnDestroyEventPublished;
      interactionEventChannel.OnEventPublished -= OnInteractionEventPublished;
      questStartedEventChannel.OnEventPublished -= OnQuestStartedEventPublished;
      questEndedEventChannel.OnEventPublished -= OnQuestEndedEventPublished;
      for (int i = 0; i < quests.Length; i++)
      {
        quests[i].OnObjectiveCompleted -= OnObjectiveCompleted;
        quests[i].OnQuestCompleted -= OnQuestCompleted;
      }
    }

    public void SetCurrentQuestProgress(Quest questToAdd)
    {
      currentQuests.Add(questToAdd);
    }

    public void SetViews(CurrentQuestView currentQuestView,
                         CurrentObjectiveView currentObjective1View,
                         CurrentObjectiveView currentObjective2View,
                         CurrentObjectiveView currentObjective3View)
    {
      this.currentQuestView = currentQuestView;
      this.currentObjective1View = currentObjective1View;
      this.currentObjective2View = currentObjective2View;
      this.currentObjective3View = currentObjective3View;
      UpdateViews();
    }

    #endregion

    #region Quest Events

    private void OnQuestCompleted(Quest completedQuest)
    {
      for (int i = 0; i < quests.Length; i++)
      {
        if (quests[i] == completedQuest)
        {
          questEndedEventChannel.Publish(new QuestEndedEvent(i));
        }

        for (int j = 0; j < completedQuest.NextQuestsIndexes.Length; j++)
        {
          if (quests[i] == quests[completedQuest.NextQuestsIndexes[j]] && !quests[i].IsCompleted)
          {
            currentQuests.Add(quests[i]);
            quests[i].QuestStarted();
            questStartedEventChannel.Publish(new QuestStartedEvent(i));        
          }
        }
      }

      currentQuests.Remove(completedQuest);
      if (activeQuest == completedQuest && currentQuests.Count > 0)
      {
        activeQuest = currentQuests[0];
      }
      else if (currentQuests.Count <= 0)
      {
        activeQuest = null;
      }

      //TODO: donner xp et gold
      UpdateViews();
    }

    private void OnObjectiveCompleted(Objective completedObjective)
    {
      UpdateViews();
    }

    #endregion

    #region Event Channels

    private void OnExplorationEventPublished(ExplorationEvent explorationEvent)
    {
      for (int i = 0; i < currentQuests.Count; i++)
      {
        currentQuests[i].ExplorationTriggered(explorationEvent.ExplorationNodeIndex);
      }
    }

    private void OnInteractionEventPublished(InteractionEvent interactionEvent)
    {
      for (int i = 0; i < currentQuests.Count; i++)
      {
        currentQuests[i].InteractionTriggered(interactionEvent.QuestInteractorIndex);
      }
    }

    private void OnDestroyEventPublished(DestroyEvent destroyEvent)
    {
      for (int i = 0; i < currentQuests.Count; i++)
      {
      }
    }

    private void OnItemAcquiredEventPublished(ItemAcquiredEvent itemEvent)
    {
      for (int i = 0; i < currentQuests.Count; i++)
      {
      }
    }

    private void OnQuestEndedEventPublished(QuestEndedEvent endedQuest)
    {
      currentQuests.Remove(quests[endedQuest.EndedQuestIndex]);
      if (activeQuest == quests[endedQuest.EndedQuestIndex] && currentQuests.Count > 0)
      {
        activeQuest = currentQuests[0];
      }
      else
      {
        activeQuest = null;
      }
      UpdateViews();
    }

    private void OnQuestStartedEventPublished(QuestStartedEvent startedQuest)
    {
      for (int i = 0; i < quests.Length; i++)
      {
        if (quests[i] == quests[startedQuest.StartedQuestIndex] && !quests[i].IsCompleted &&
            !currentQuests.Contains(quests[i]))
        {
          currentQuests.Add(quests[startedQuest.StartedQuestIndex]);
          quests[startedQuest.StartedQuestIndex].QuestStarted();
        }
      }
      if (activeQuest == null && currentQuests.Count > 0)
      {
        activeQuest = currentQuests[0];
      }
      UpdateViews();
    }

    #endregion

    #region UI

    private void UpdateViews()
    {
      if (activeQuest != null)
      {
        currentQuestView.QuestEnabled = true;
        currentQuestView.SetQuest(activeQuest.QuestName);

        if (activeQuest.CurrentObjectives.Count > 0)
        {
          currentObjective1View.ObjectiveEnabled = true;
          currentObjective1View.SetObjective(activeQuest.CurrentObjectives[0].ObjectiveDescription);
        }
        else
        {
          currentObjective1View.ObjectiveEnabled = false;
        }

        if (activeQuest.CurrentObjectives.Count > 1)
        {
          currentObjective2View.ObjectiveEnabled = true;
          currentObjective2View.SetObjective(activeQuest.CurrentObjectives[1].ObjectiveDescription);
        }
        else
        {
          currentObjective2View.ObjectiveEnabled = false;
        }

        if (activeQuest.CurrentObjectives.Count > 2)
        {
          currentObjective3View.ObjectiveEnabled = true;
          currentObjective3View.SetObjective(activeQuest.CurrentObjectives[2].ObjectiveDescription);
        }
        else
        {
          currentObjective3View.ObjectiveEnabled = false;
        }
      }
      else
      {
        currentQuestView.QuestEnabled = false;
        currentObjective1View.ObjectiveEnabled = false;
        currentObjective2View.ObjectiveEnabled = false;
        currentObjective3View.ObjectiveEnabled = false;
      }
    }

    #endregion
  }
}