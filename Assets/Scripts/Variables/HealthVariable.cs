using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(menuName = "Variables/Health")]
    public class HealthVariable : GenericVariable<Vector2Int>
    {
        public int Health
        {
            get => Value.x;
            set
            {
                var v = Value;
                v.x = value;
                Value = v;
            }
        }

        public int MaxHealth
        {
            get => Value.y;
            set
            {
                var v = Value;
                v.y = value;
                Value = v;
            }
        }
    }
}