using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(menuName = "Variables/Bool")]
    public class BoolVariable : GenericVariable<bool>
    {
        protected override bool SupportsPlayerPrefs => true;

        protected override bool GetFromPlayerPrefs(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        protected override void StoreInPlayerPrefs(string key, bool actualValue)
        {
            PlayerPrefs.SetInt(key, actualValue ? 1 : 0);
        }
    }
}