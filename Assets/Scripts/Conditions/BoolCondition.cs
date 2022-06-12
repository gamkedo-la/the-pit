using System;
using System.Linq;
using UnityEngine;
using Variables;

namespace Conditions
{
    [CreateAssetMenu(menuName = "Conditions/Bool")]
    public class BoolCondition : Condition
    {
        public Function function;
        public BoolVariable[] variables;

        [Serializable]
        public enum Function
        {
            And, Or, Nand, Nor
        }

        public override bool Evaluate()
        {
            return function == Function.And
                    ? variables.All(v => v.Value)
                    : variables.Any(v => v.Value)
                ;
        }
    }
}