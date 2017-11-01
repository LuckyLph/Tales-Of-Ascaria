using UnityEngine;

namespace TalesOfAscaria
{
  public class AdjustDepthFromHeight : GameScript
  {
    [SerializeField]
    private Transform visualTransform;

    private Transform parentTransform;

    private void Awake()
    {
      parentTransform = transform.parent.transform;
    }

    private void Update()
    {
      visualTransform.position = new Vector3(visualTransform.position.x, visualTransform.position.y, parentTransform.position.y);
    }
  }
}