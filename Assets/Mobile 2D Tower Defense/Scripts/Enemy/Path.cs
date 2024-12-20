using UnityEngine;

public class Path : MonoBehaviour
{
    [System.Serializable]
    public class WayPoint
    {
        public Transform[] wayPoints;
    }
    public WayPoint[] ways;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (WayPoint way in ways)
        {
            for (int i = 0; i < way.wayPoints.Length - 1; i++)
            {
                Gizmos.DrawLine(way.wayPoints[i].position, way.wayPoints[i + 1].position);
            }
        }
    }

}
