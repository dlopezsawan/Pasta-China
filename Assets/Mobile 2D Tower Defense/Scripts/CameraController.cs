using System.Collections;
using System.Collections.Generic;
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
            private Vector3 dragCurrentPosition;
            private Vector3 newPosition;

            void Start()
            {
                newPosition = transform.position;
            }

            void Update()
            {
                
                if(Input.GetMouseButtonDown(0))
                {
                    Plane plane = new Plane(Vector3.forward, Vector3.zero);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float entry;
                    if(plane.Raycast(ray, out entry))
                    {
                        dragStartPosition = ray.GetPoint(entry);
                    }
                }
                else if(Input.GetMouseButton(0))
                {
                    Plane plane = new Plane(Vector3.forward, Vector3.zero);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float entry;
                    if(plane.Raycast(ray, out entry))
                    {
                        dragCurrentPosition = ray.GetPoint(entry);

                        newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                    }
                }
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, bottomLimit, upperLimit), transform.position.z);
            }

            private void OnDrawGizmosSelected()
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(rightLimit, upperLimit));
                Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
                Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(leftLimit, bottomLimit));
                Gizmos.DrawLine(new Vector2(rightLimit, upperLimit), new Vector2(rightLimit, bottomLimit));
            }
    }
}
