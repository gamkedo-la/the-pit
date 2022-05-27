using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(menuName = "Variables/Int")]
    public class IntVariable : GenericVariable<int>
    {
        protected override bool SupportsPlayerPrefs => true;

        protected override int GetFromPlayerPrefs(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        protected override void StoreInPlayerPrefs(string key, int actualValue)
        {
            PlayerPrefs.SetInt(key, actualValue);
        }
    }
}