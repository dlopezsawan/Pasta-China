using UnityEngine;

namespace MobileTowerDefense
{
    public class CameraController : MonoBehaviour
    {
        public float movementTime;

        [Header("Limits")]
        public float leftLimit;
        public float rightLimit;
        public float bottomLimit;
        public float upperLimit;

        private Vector3 dragStartPosition;
        private Vector3 newPosition;

        void Start()
        {
            newPosition = transform.position;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragStartPosition = GetMouseWorldPosition();
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 dragCurrentPosition = GetMouseWorldPosition();
                Vector3 dragDelta = dragStartPosition - dragCurrentPosition;
                newPosition = transform.position + dragDelta;
            }

            // Smooth camera movement
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);

            // Clamp position to limits
            ClampPosition();
        }

        private Vector3 GetMouseWorldPosition()
        {
            // Convert screen point to world point
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(Camera.main.transform.position.z); // Ensure correct depth
            return Camera.main.ScreenToWorldPoint(mousePosition);
        }

        private void ClampPosition()
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
                transform.position.z
            );
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(leftLimit, upperLimit, 0), new Vector3(rightLimit, upperLimit, 0));
            Gizmos.DrawLine(new Vector3(leftLimit, bottomLimit, 0), new Vector3(rightLimit, bottomLimit, 0));
            Gizmos.DrawLine(new Vector3(leftLimit, upperLimit, 0), new Vector3(leftLimit, bottomLimit, 0));
            Gizmos.DrawLine(new Vector3(rightLimit, upperLimit, 0), new Vector3(rightLimit, bottomLimit, 0));
        }
    }
}
