using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/Event/CreationEventPublisher")]
  public class CreationEventPublisher : GameScript
  {
    [SerializeField]
    private R.E.Prefab prefab;

    private GameObject topParent;
    private CreationEventChannel eventChannel;

    private void InjectCreationEventPublisher([ParentScope] GameObject topParent,
                                              [EventChannelScope] CreationEventChannel eventChannel)
    {
      this.topParent = topParent;
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectCreationEventPublisher");

      eventChannel.Publish(new CreationEvent(prefab, topParent));
    }
  }
}