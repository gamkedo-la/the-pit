using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(menuName = "Variables/Float")]
    public class FloatVariable : GenericVariable<float>
    {
        public override bool SupportsPlayerPrefs => true;
        public override float GetFromPlayerPrefs(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public override void StoreInPlayerPrefs(string key, float actualValue)
        {
            PlayerPrefs.SetFloat(key, actualValue);
        }
    }
}