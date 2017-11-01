using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Object/Control/CameraController")]
  public class CameraController : GameScript
  {
    public bool HasMapBounds { get; set; }

    [Header("Camera zoom adjustments settings")]
    [Tooltip("Left Margin for the camera's expansion")]
    [SerializeField]
    private float cameraExpandLeftMargin;
    [Tooltip("Right Margin for the camera's expansion")]
    [SerializeField]
    private float cameraExpandRightMargin;
    [Tooltip("Bottom Margin for the camera's expansion")]
    [SerializeField]
    private float cameraExpandBottomMargin;
    [Tooltip("Top Margin for the camera's expansion")]
    [SerializeField]
    private float cameraExpandTopMargin;
    [Tooltip("Left Margin for the camera's decrease")]
    [SerializeField]
    private float cameraDecreaseLeftMargin;
    [Tooltip("Left Margin for the camera's decrease")]
    [SerializeField]
    private float cameraDecreaseRightMargin;
    [Tooltip("Right Margin for the camera's decrease")]
    [SerializeField]
    private float cameraDecreaseBottomMargin;
    [Tooltip("Bottom Margin for the camera's decrease")]
    [SerializeField]
    private float cameraDecreaseTopMargin;
    [Tooltip("Top Margin for the camera's decrease")]
    [SerializeField]
    private float cameraMaxSizeIncrease;
    [Tooltip("Maximum ortographic size the camera can expand to")]
    [SerializeField]
    private float cameraSizeChangeSpeed;

    [Space(10)]
    [Tooltip("The speed at which the camera tracks the players in units per second")]
    [SerializeField]
    private float speed;

    private CameraMover cameraMover;
    private new Camera camera;
    private PlayersCenterOfMass centerOfMass;
    private PlayersList playersList;
    private Vector2 target;
    private Transform topLeft;
    private Transform bottomRight;

    private float cameraMaximumSize = 0;
    private float cameraMinimumSize = 0;

    private void InjectCameraController([EntityScope] CameraMover cameraMover,
                                       [TagScope(R.S.Tag.MainCamera)] Camera camera,
                                       [ApplicationScope] PlayersCenterOfMass centerOfMass,
                                       [ApplicationScope] PlayersList playersList)
    {
      this.cameraMover = cameraMover;
      this.camera = camera;
      this.centerOfMass = centerOfMass;
      this.playersList = playersList;
    }

    private void Awake()
    {
      InjectDependencies("InjectCameraController");

      target = centerOfMass.CenterOfMass;
      SetCameraSize();
      cameraMinimumSize = camera.orthographicSize;
      cameraMaximumSize = camera.orthographicSize + cameraMaxSizeIncrease;
    }

    private void Update()
    {
      if (HasMapBounds)
      {
        SetCameraSize();
        target = centerOfMass.CenterOfMass;
        GameObject[] players = playersList.PlayersAlive.ToArray();
        bool mustDecrease = true;
        float oldCameraSize = 0;
        float cameraHeight = 2f * camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;

        for (int i = 0; i < players.Length; i++)
        {
          Vector3 screenPosition = camera.WorldToViewportPoint(players[i].transform.position);
          if (CameraMustZoomOut(screenPosition))
          {
            oldCameraSize = camera.orthographicSize;
            camera.orthographicSize += cameraSizeChangeSpeed;
            UpdateCameraInfo(out cameraWidth, out cameraHeight);
            if (!CheckCameraBounds(camera.transform.position, cameraWidth, cameraHeight))
            {
              camera.orthographicSize = oldCameraSize;
            }
          }
          if (!CameraMustZoomIn(screenPosition))
          {
            mustDecrease = false;
          }
        }
        if (mustDecrease)
        {
          camera.orthographicSize -= cameraSizeChangeSpeed;
        }
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, cameraMinimumSize, cameraMaximumSize);

        Vector3 nextPosition = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        if (CheckCameraXBounds(nextPosition, cameraWidth))
        {
          cameraMover.MoveCamera(new Vector3(nextPosition.x, camera.transform.position.y, camera.transform.position.z));
        }
        if (CheckCameraYBounds(nextPosition, cameraHeight))
        {
          cameraMover.MoveCamera(new Vector3(camera.transform.position.x, nextPosition.y, camera.transform.position.z));
        }
      }
    }

    public void SetMapBounds(Transform topLeft, Transform bottomRight)
    {
      this.topLeft = topLeft;
      this.bottomRight = bottomRight;
      HasMapBounds = true;
    }

    private bool CameraMustZoomOut(Vector2 screenPosition)
    {
      return screenPosition.x < cameraExpandLeftMargin || screenPosition.x > cameraExpandRightMargin ||
             screenPosition.y < cameraExpandBottomMargin || screenPosition.y > cameraExpandTopMargin;
    }

    private bool CameraMustZoomIn(Vector2 screenPosition)
    {
      return screenPosition.x > cameraDecreaseLeftMargin && screenPosition.x < cameraDecreaseRightMargin &&
             screenPosition.y > cameraDecreaseBottomMargin && screenPosition.y < cameraDecreaseTopMargin;
    }

    private bool CheckCameraXBounds(Vector3 nextPosition, float cameraWidth)
    {
      return nextPosition.x + cameraWidth / 2 < bottomRight.position.x && nextPosition.x - cameraWidth / 2 > topLeft.position.x;
    }

    private bool CheckCameraYBounds(Vector3 nextPosition, float cameraHeight)
    {
      return nextPosition.y + cameraHeight / 2 < topLeft.position.y && nextPosition.y - cameraHeight / 2 > bottomRight.position.y;
    }

    private bool CheckCameraBounds(Vector3 nextPosition, float cameraWidth, float cameraHeight)
    {
      return nextPosition.x + cameraWidth / 2 < bottomRight.position.x && nextPosition.x - cameraWidth / 2 > topLeft.position.x &&
             nextPosition.y + cameraHeight / 2 < topLeft.position.y && nextPosition.y - cameraHeight / 2 > bottomRight.position.y;
    }

    private void UpdateCameraInfo(out float cameraWidth, out float cameraHeight)
    {
      cameraHeight = 2f * camera.orthographicSize;
      cameraWidth = cameraHeight * camera.aspect;
    }

    private void SetCameraSize()
    {
      camera.orthographicSize = Screen.height / 100f / 6f;
    }
  }
}