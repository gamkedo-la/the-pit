using UnityEngine;

namespace Conditions
{
    [CreateAssetMenu(menuName = "Conditions/Not")]
    public class NotCondition : Condition
    {
        public Condition condition;

        public override bool Evaluate()
        {
            return !condition.Evaluate();
        }
    }
}