using Harmony;

namespace TalesOfAscaria
{
    public class CameraSizeChangedEvent : IEvent
    {
        public float OrthographicSize { get; private set; }
        public float Aspect { get; private set; }

        public CameraSizeChangedEvent(float size, float aspect)
        {
            OrthographicSize = size;
            Aspect = aspect;
        }
    }
}