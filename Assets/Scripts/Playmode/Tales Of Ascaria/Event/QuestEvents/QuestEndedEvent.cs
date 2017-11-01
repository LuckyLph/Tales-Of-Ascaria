using Harmony;

namespace TalesOfAscaria
{
    public class QuestEndedEvent : IEvent
    {
        public int EndedQuestIndex { get; private set; }

        public QuestEndedEvent(int endedQuest)
        {
            EndedQuestIndex = endedQuest;
        }
    }
}