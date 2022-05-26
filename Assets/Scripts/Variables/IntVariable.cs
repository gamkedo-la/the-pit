using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(menuName = "Variables/Int")]
    public class IntVariable : GenericVariable<int>
    {
        public override bool SupportsPlayerPrefs => true;
        public override int GetFromPlayerPrefs(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public override void StoreInPlayerPrefs(string key, int actualValue)
        {
            PlayerPrefs.SetInt(key, actualValue);
        }
    }
}