using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(menuName = "Variables/Bool")]
    public class BoolVariable : GenericVariable<bool>
    {
        public override bool SupportsPlayerPrefs => true;
        public override bool GetFromPlayerPrefs(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public override void StoreInPlayerPrefs(string key, bool actualValue)
        {
            PlayerPrefs.SetInt(key, actualValue ? 1 : 0);
        }
    }
}