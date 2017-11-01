using Harmony;

namespace TalesOfAscaria
{
    public class InteractionEvent : IEvent
    {
        public InteractionObjective.InteractionObjectives QuestInteractorIndex { get; private set; }

        public InteractionEvent(InteractionObjective.InteractionObjectives questInteractorIndex)
        {
            QuestInteractorIndex = questInteractorIndex;
        }
    }
}