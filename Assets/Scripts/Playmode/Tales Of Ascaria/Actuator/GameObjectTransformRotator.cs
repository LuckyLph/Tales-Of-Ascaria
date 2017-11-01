using UnityEngine;

namespace TalesOfAscaria
{
  /// <summary>
  /// Effectue une rotation sur le Transform du Top Parent de l'objet
  /// </summary>
  public class GameObjectTransformRotator : GameScript
  {
    [Tooltip("The rotation speed in degrees per second. Can be negative for counter-clockwise rotation.")]
    [SerializeField]
    private float rotationSpeed;

    private Transform objectToRotate;

    private void Awake()
    {
      objectToRotate = GetRoot().transform;
    }


    private void Update()
    {
      objectToRotate.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
  }
}