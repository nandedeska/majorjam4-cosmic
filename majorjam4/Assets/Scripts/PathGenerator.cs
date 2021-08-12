using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    Transform[] points;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        points = GetComponentsInChildren<Transform>();
        waypoints.Clear();

        foreach(Transform point in points)
        {
            if(point != transform)
            {
                waypoints.Add(point);
            }
        }

        for (int i = 0; i < waypoints.Count; i++)
        {
            Vector2 pos = waypoints[i].position;
            if (i > 0)
            {
                Vector2 prevPos = waypoints[i - 1].position;
                Gizmos.DrawLine(prevPos, pos);
                Gizmos.DrawWireSphere(pos, 0.25f);
            }
        }
    }
}
