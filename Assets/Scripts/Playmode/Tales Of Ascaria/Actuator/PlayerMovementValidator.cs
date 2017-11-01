using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class PlayerMovementValidator : GameScript
  {
    private new Camera camera;
    private Vector2 cameraPosition;
    private Vector2 nextPosition;
    private float cameraHeight;
    private float cameraWidth;

    private void InjectPlayerMovementValidator([TagScope(R.S.Tag.MainCamera)] Camera camera)
    {
      this.camera = camera;
    }

    private void Awake()
    {
      InjectDependencies("InjectPlayerMovementValidator");
    }

    public bool ValidateUpMovement(Vector2 currentPosition, Vector2 movementVector, float playerSizeY, float speed)
    {
      UpdateCameraInfo();
      nextPosition = currentPosition + movementVector * speed * Time.deltaTime;
      return nextPosition.y + playerSizeY < cameraPosition.y + cameraHeight / 2;
    }

    public bool ValidateDownMovement(Vector2 currentPosition, Vector2 movementVector, float playerSizeY, float speed)
    {
      UpdateCameraInfo();
      nextPosition = currentPosition + movementVector * speed * Time.deltaTime;
      return nextPosition.y > cameraPosition.y - cameraHeight / 2;
    }

    public bool ValidateLeftMovement(Vector2 currentPosition, Vector2 movementVector, float playerSizeX, float speed)
    {
      UpdateCameraInfo();
      nextPosition = currentPosition + movementVector * speed * Time.deltaTime;
      return nextPosition.x - playerSizeX / 3 > cameraPosition.x - cameraWidth / 2;
    }

    public bool ValidateRightMovement(Vector2 currentPosition, Vector2 movementVector, float playerSizeX, float speed)
    {
      UpdateCameraInfo();
      nextPosition = currentPosition + movementVector * speed * Time.deltaTime;
      return nextPosition.x + playerSizeX / 3 < cameraPosition.x + cameraWidth / 2;
    }

    public bool IsPlayerOutsideCamera(Vector2 currentPosition, Vector2 playerSize)
    {
      UpdateCameraInfo();
      return currentPosition.x + playerSize.x / 2 < cameraPosition.x - cameraWidth / 2 ||
             currentPosition.x - playerSize.x / 2 > cameraPosition.x + cameraWidth / 2 ||
             currentPosition.y - playerSize.y / 2 > cameraPosition.y + cameraHeight / 2 ||
             currentPosition.y + playerSize.y / 2 < cameraPosition.y - cameraHeight / 2;
    }

    private void UpdateCameraInfo()
    {
      cameraPosition = camera.transform.position;
      cameraHeight = 2f * camera.orthographicSize;
      cameraWidth = cameraHeight * camera.aspect;
    }

    private void UpdateNextPosition(Vector2 currentPosition, Vector2 movementVector, float speed)
    {
      nextPosition = currentPosition + movementVector * speed * Time.deltaTime;
    }
  }
}