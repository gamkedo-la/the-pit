using UnityEngine;

namespace Enemy
{
    public class TentacleMovement : MonoBehaviour
    {
        public Transform ikTarget;
        public Transform basePoint;

        public Rect border;
        
        public float harmonic1 = 83f;
        public float harmonic2 = 93f;
        public float harmonic3 = 219f;

        public float randomMoveSpeed = 1f;

        private float maxRange;
        private Vector2 min, max;

        private Vector3 origin;
        private Vector2 direction;
        public float distance;
        private float speed;
        
        private void Start()
        {
            maxRange = Vector3.Distance(ikTarget.transform.position, basePoint.transform.position);
            min = border.position - border.size / 2f;
            max = border.position + border.size / 2f;
        }

        public void MoveTo(Vector2 position, float desiredSpeed)
        {
            origin = ikTarget.transform.position;
            direction = position - (Vector2)origin;
            distance = direction.magnitude;
            if (distance > 1e-3f)
            {
                direction /= distance;
            }
            else
            {
                distance = 0f;
            }

            speed = desiredSpeed;
        }

        public void MoveToRandomPoint()
        {
            var range = Mathf.Lerp(maxRange * 0.5f, maxRange * 0.7f,
                (Mathf.Sin(harmonic3 * Time.time * Mathf.Deg2Rad)+1)/2f);
            var x = Mathf.Cos(harmonic1 * Time.time * Mathf.Deg2Rad) * range;
            var y = Mathf.Sin(harmonic2 * Time.time * Mathf.Deg2Rad) * range;

            x = Mathf.Clamp(x, min.x, max.x);
            y = Mathf.Clamp(y, min.y, max.y);

            var pos = basePoint.transform.position;
            pos.x += x;
            pos.y += y;
            MoveTo(pos, randomMoveSpeed);
        }

        private void Update()
        {
            if (distance > 1e-3f)
            {
                var distanceToMove = Mathf.Min(distance, speed * Time.deltaTime);
                ikTarget.transform.position += (Vector3)(direction * distanceToMove);
                distance -= distanceToMove;
            }
            else
            {
                distance = 0f;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(basePoint.transform.position + (Vector3)border.position, border.size);
        }
    }

}