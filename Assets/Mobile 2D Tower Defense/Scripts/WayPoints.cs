using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class WayPoints : MonoBehaviour
    {
        [System.Serializable]
        public class WayPoint
        {
            public Transform spawnPoint;
            public Transform[] wayPoints;
        }
        public WayPoint[] ways;

        private void OnDrawGizmos()
        {
            foreach(WayPoint way in ways)
            {
                Gizmos.color = Color.yellow;
                for(int i = 0; i < way.wayPoints.Length - 1; i++)
                {
                    Gizmos.DrawLine(way.wayPoints[i].position, way.wayPoints[i + 1].position);
                }
            }
        }
    }
}

