using System.Collections;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class SwingMoveable : Moveable
  {
    public enum RotationDirection
    {
      Clockwise,
      Counterclockwise
    }

    [Tooltip("Le nombre de degrés effectués.")]
    [SerializeField] private float numberOfDegrees;

    [Tooltip("La direction de la rotation")]
    [SerializeField] private RotationDirection direction;

    private float degreesPerSeconds;
    private float currentDegrees;

    public override void ExecuteMove()
    {
      degreesPerSeconds = numberOfDegrees / timeTaken;
      currentDegrees = 0f;
      StartCoroutine(Swing());
    }

    private IEnumerator Swing()
    {
      TriggerStart();
      while (currentDegrees < numberOfDegrees)
      {
        float degreesThisFrame = degreesPerSeconds * Time.deltaTime;
        transform.Rotate(0f, 0f, degreesThisFrame * (direction == RotationDirection.Clockwise ? -1 : 1));
        currentDegrees += degreesThisFrame;
        yield return null;
      }
      TriggerEnd();
    }
  }
}