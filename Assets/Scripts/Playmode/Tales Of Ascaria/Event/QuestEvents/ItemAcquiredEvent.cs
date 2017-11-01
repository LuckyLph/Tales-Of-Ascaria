using Harmony;

namespace TalesOfAscaria
{
    public class ItemAcquiredEvent : IEvent
    {
        public Item AcquiredItem { get; private set; }

        public ItemAcquiredEvent(Item acquiredItem)
        {
            AcquiredItem = acquiredItem;
        }
    }
}