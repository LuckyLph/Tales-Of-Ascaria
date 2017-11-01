using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Ui/Control/LoadingScreenController")]
  public class LoadingScreenController : GameScript
  {
    private Canvas loadingScreenCanvas;
    private ActivityStack activityStack;

    private void InjectLoadingScreenController([EntityScope] Canvas loadingScreenCanvas,
                                              [ApplicationScope] ActivityStack activityStack)
    {
      this.loadingScreenCanvas = loadingScreenCanvas;
      this.activityStack = activityStack;
    }

    private void Awake()
    {
      InjectDependencies("InjectLoadingScreenController");

      loadingScreenCanvas.enabled = false;
    }

    private void OnEnable()
    {
      activityStack.OnActivityLoadingStarted += OnActivityLoadStart;
      activityStack.OnActivityLoadingEnded += OnActivityLoadEnd;
    }

    private void OnDisable()
    {
      activityStack.OnActivityLoadingStarted -= OnActivityLoadStart;
      activityStack.OnActivityLoadingEnded -= OnActivityLoadEnd;
    }

    private void OnActivityLoadStart()
    {
      loadingScreenCanvas.enabled = true;
    }

    private void OnActivityLoadEnd()
    {
      loadingScreenCanvas.enabled = false;
    }
  }
}