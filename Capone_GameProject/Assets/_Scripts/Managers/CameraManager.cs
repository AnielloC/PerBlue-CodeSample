using Assets._Scripts.Levels;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        // Event for when the camera is done moving to a new level segment
        public UnityEvent<int> OnCameraStoppedMoving { get; set; } = new UnityEvent<int>();

        // Level controller currently in use
        [SerializeField] private LevelControllerBase levelController;

        // If the camera should be moving
        private bool moveCamera = false;

        // Target position for the camera
        private Vector3 targetPos;

        // Set the targetPos and prepare the camera for movement
        public void MoveCamera()
        {
            targetPos = levelController.LevelSegments.Find($"Targets{levelController.curLevelSegment}/CameraTarget").localPosition;
            moveCamera = true;
        }

        private void Update()
        {
            if (moveCamera)
            {
                if (Vector3.Distance(transform.localPosition, targetPos) > 0.1f)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * 15);
                }
                else
                {
                    moveCamera = false;
                    OnCameraStoppedMoving?.Invoke(levelController.curLevelSegment);
                }
            }
        }

        private void OnDestroy()
        {
            OnCameraStoppedMoving?.RemoveAllListeners();
        }
    }
}
