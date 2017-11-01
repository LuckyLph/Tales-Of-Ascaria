using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class CameraMover : GameScript
  {
    private new Camera camera;

    private void InjectCameraMover([ParentScope] Camera camera)
    {
      this.camera = camera;
    }

    private void Awake()
    {
      InjectDependencies("InjectCameraMover");
    }

    public void MoveCamera(Vector3 movement)
    {
      camera.transform.position = movement;
    }
  }
}