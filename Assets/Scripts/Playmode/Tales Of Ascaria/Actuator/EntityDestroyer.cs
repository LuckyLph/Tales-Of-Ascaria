using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void EntityDestroyedEventHandler();

  [AddComponentMenu("Game/World/Object/Actuator/EntityDestroyer")]
  public class EntityDestroyer : GameScript
  {
    private GameObject parent;

    public virtual event EntityDestroyedEventHandler OnDestroyed;

    private void InjectEntityDestroyer([ParentScope] GameObject parent)
    {
      this.parent = parent;
    }

    private void Awake()
    {
      InjectDependencies("InjectEntityDestroyer");
    }

    [CalledOutsideOfCode]
    public virtual void Destroy()
    {
      Destroy(parent);

      if (OnDestroyed != null) OnDestroyed();
    }

    [CalledOutsideOfCode]
    public virtual void Destroy(float delay, GameObject parent)
    {
      Destroy(parent, delay);

      if (OnDestroyed != null) OnDestroyed();
    }
  }
}