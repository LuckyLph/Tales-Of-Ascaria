﻿using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
    [AddComponentMenu("Game/World/Object/Actuator/ScreenPositionProvider")]
    public class ScreenPositionProvider : GameScript
    {
        private static readonly Vector2 ScreenCenter = new Vector2(0.5f, 0.5f);

        private new Camera camera;

        public void InjectOutOfScreenSpawner([TagScope(R.S.Tag.MainCamera)] Camera camera,
                                             [ApplicationScope] Random random)
        {
            this.camera = camera;
        }

        public void Awake()
        {
            InjectDependencies("InjectOutOfScreenSpawner");
        }

        public virtual Vector2 GetRandomInScreenPosition()
        {
            return camera.ViewportToWorldPoint(RandomExtensions.GetRandomPosition(0, 1, 0, 1));
        }

        public virtual Vector2 GetRandomOffScreenPosition(float objectRadius)
        {
            float objectRadiusInViewport = GetRadiusInViewport(objectRadius);

            //Viewport X position start at 0 and ends at 1
            //Viewport Y position start at 0 and ends at 1
            //Rectangle center is thus at position (0.5, 0.5)
            //Height is thus 1 and Width is also 1
            Vector2 viewportPosition = RandomExtensions.GetRandomPositionOnRectangleEdge(ScreenCenter,
                                                                                         1 + objectRadiusInViewport * 2, //Times 2, for Up and Down margin
                                                                                         1 + objectRadiusInViewport * 2); //Times 2, for Left and Right margin
            return camera.ViewportToWorldPoint(viewportPosition);
        }

        private float GetRadiusInViewport(float radius)
        {
            return camera.WorldToViewportPoint(camera.ScreenToWorldPoint(Vector2.zero) + Vector3.one * radius).x;
        }
    }
}