using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(menuName = "Variables/Float")]
    public class FloatVariable : GenericVariable<float>
    {
        protected override bool SupportsPlayerPrefs => true;

        protected override float GetFromPlayerPrefs(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        protected override void StoreInPlayerPrefs(string key, float actualValue)
        {
            PlayerPrefs.SetFloat(key, actualValue);
        }
    }
}