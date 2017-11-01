using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class ProjectileController : GameScript
  {
    public Vector3 Direction
    {
      get { return direction; }
      set { direction = value; }
    }

    [Tooltip("Projectile speed in units per second")]
    [SerializeField] private float speed;

    [Tooltip("Duration of the projectile's life in seconds")]
    [SerializeField] private float duration;

    private Vector3 direction = Vector3.right;
    private TranslateMover mover;

    private void InjectProjectileController([EntityScope] TranslateMover mover)
    {
      this.mover = mover;
    }

    /// <summary>
    /// Awakes this instance.
    /// </summary>
    private void Awake()
    {
      InjectDependencies("InjectProjectileController");
      Destroy(gameObject.transform.root.gameObject, duration);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
      MoveStep();
    }

    /// <summary>
    /// Déplace le projectile vers sa direction.
    /// </summary>
    public void MoveStep()
    {
      mover.Move(direction.normalized, speed * Time.deltaTime);
    }
  }
}
