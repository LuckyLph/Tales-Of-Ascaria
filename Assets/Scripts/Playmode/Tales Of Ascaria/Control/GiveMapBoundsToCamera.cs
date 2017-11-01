using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class GiveMapBoundsToCamera : GameScript
  {
    private CameraController cameraController;
    private Transform topLeft;
    private Transform bottomRight;

    private void InjectGiveMapBoundsToCamera([TagScope(R.S.Tag.MainCamera)] CameraController cameraController,
                                             [TagScope(R.S.Tag.MapTopLeft)] GameObject topLeft,
                                             [TagScope(R.S.Tag.MapBottomRight)] GameObject bottomRight)
    {
      this.cameraController = cameraController;
      this.topLeft = topLeft.transform;
      this.bottomRight = bottomRight.transform;
    }

    private void Awake()
    {
      InjectDependencies("InjectGiveMapBoundsToCamera");
    }

    private void Start()
    {
      cameraController.SetMapBounds(topLeft, bottomRight);
    }
  }
}