using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(menuName = "Variables/Float (Min, Max)")]
    public class MinMaxFloatVariable : FloatVariable
    {
        [SerializeField]
        private float minValue;
        [SerializeField]
        private float maxValue;

        public float MinValue
        {
            get => minValue;
            set => minValue = value;
        }

        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }

        public float Ratio => (Value - MinValue) / (MaxValue - MinValue);

        public void Add(float delta)
        {
            Value = delta switch
            {
                > 0 => Mathf.Min(Value + delta, maxValue),
                < 0 => Mathf.Max(Value + delta, minValue),
                _ => Value
            };
        }

        public void Subtract(float delta)
        {
            Add(-delta);
        }
    }
}