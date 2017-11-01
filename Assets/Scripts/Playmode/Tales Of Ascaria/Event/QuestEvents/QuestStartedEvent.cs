using Harmony;

namespace TalesOfAscaria
{
  public class QuestStartedEvent : IEvent
  {
    public int StartedQuestIndex { get; private set; }

    public QuestStartedEvent(int startedQuest)
    {
      StartedQuestIndex = startedQuest;
    }
  }
}