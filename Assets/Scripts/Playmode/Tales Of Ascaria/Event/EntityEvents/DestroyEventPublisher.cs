using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/Event/DestroyEventPublisher")]
  public class DestroyEventPublisher : GameScript
  {
    [SerializeField]
    private R.E.Prefab prefab;

    private GameObject topParent;
    private EntityDestroyer entityDestroyer;
    private DestroyEventChannel eventChannel;

    private void InjectDestroyEventPublisher([ParentScope] GameObject topParent,
                                             [SiblingsScope] EntityDestroyer entityDestroyer,
                                             [EventChannelScope] DestroyEventChannel eventChannel)
    {
      this.topParent = topParent;
      this.entityDestroyer = entityDestroyer;
      this.eventChannel = eventChannel;
    }

    private void Awake()
    {
      InjectDependencies("InjectDestroyEventPublisher");

      entityDestroyer.OnDestroyed += OnEntityDestroyed;
    }

    private void OnDestroy()
    {
      entityDestroyer.OnDestroyed -= OnEntityDestroyed;
    }

    private void OnEntityDestroyed()
    {
      eventChannel.Publish(new DestroyEvent(prefab, topParent));
    }
  }
}