using UnityEngine;

namespace Util
{
    public static class GizmoHelper
    {
        public static void DrawArc(Vector3 origin, float radius, float degrees1, float degrees2)
        {
            var p1 = origin + radius * new Vector3(Mathf.Cos(degrees1 * Mathf.Deg2Rad), Mathf.Sin(degrees1 * Mathf.Deg2Rad), 0);
            var p2 = origin + radius * new Vector3(Mathf.Cos(degrees2 * Mathf.Deg2Rad), Mathf.Sin(degrees2 * Mathf.Deg2Rad), 0);
            Gizmos.DrawLine(origin, p1);
            Gizmos.DrawLine(origin, p2);

            for (var i = 1; i <= 30; i++)
            {
                var angle = degrees1 + i * (degrees2 - degrees1) / 30f;
                p2 = origin + radius * new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                Gizmos.DrawLine(p1, p2);
                p1 = p2;
            }
        }
    }
}