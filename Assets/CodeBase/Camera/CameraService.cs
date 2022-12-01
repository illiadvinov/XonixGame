using UnityEngine;
using Zenject;

namespace CodeBase.Camera
{
    public class CameraService
    {
        public float Height { get; private set; }
        public float Width { get; private set; }

        [Inject]
        public CameraService()
        {
            if (UnityEngine.Camera.main != null)
            {
                Height = 2f * UnityEngine.Camera.main.orthographicSize;
                Width = Height * UnityEngine.Camera.main.aspect;
            }
        }

        public void SetCameraPosition()
        {
            UnityEngine.Camera.main.transform.position = new UnityEngine.Vector3(
                Mathf.FloorToInt(Width / 2) + .5f, Height / 2 - .5f, -10);
        }
    }
}