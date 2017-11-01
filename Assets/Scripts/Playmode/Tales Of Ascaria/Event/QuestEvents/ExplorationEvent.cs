using Harmony;

namespace TalesOfAscaria
{
    public class ExplorationEvent : IEvent
    {
        public ExplorationObjective.ExplorationObjectives ExplorationNodeIndex { get; private set; }

        public ExplorationEvent(ExplorationObjective.ExplorationObjectives explorationNodeIndex)
        {
            ExplorationNodeIndex = explorationNodeIndex;
        }
    }
}