using System.Collections;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class MultiHitController : GameScript
  {
    public float Duration
    {
      get { return duration; }
      set { duration = value; }
    }

    [Tooltip("Number of hits")]
    [SerializeField]
    private int numberOfHits;

    [Tooltip("Duration of the spell. Hits will be distributed evenly over the duration.")]
    [SerializeField]
    private float duration;

    private Collider2D hitbox;
    private float timeBetweenEachHit;

    private void InjectMultiHitController([EntityScope] Collider2D hitbox)
    {
      this.hitbox = hitbox;
    }

    private void Awake()
    {
      InjectDependencies("InjectMultiHitController");
      timeBetweenEachHit = duration / numberOfHits;
      hitbox.enabled = false;
    }

    private void Start()
    {
      StartCoroutine(DoMultiHit());
    }

    public IEnumerator DoMultiHit()
    {
      Debug.Log("Doing multihit!");
      for (int i = 0; i < numberOfHits; i++)
      {
        hitbox.enabled = true;
        yield return new WaitForSeconds(Time.deltaTime * 2);
        hitbox.enabled = false;
        yield return new WaitForSeconds(timeBetweenEachHit - Time.deltaTime * 2);
      }
      Destroy(gameObject.transform.root.gameObject);
    }
  }
}
