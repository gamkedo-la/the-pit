using UnityEngine;

namespace Conditions
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool Evaluate();
    }
}