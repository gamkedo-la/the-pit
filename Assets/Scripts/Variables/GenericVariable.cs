using System;
using UnityEngine;

namespace Variables
{
    public class GenericVariable<T> : ScriptableObject
    {
        [Tooltip("Current value")]
        [SerializeField] private T value;

        [Tooltip("Default value")]
        [SerializeField] private T defaultValue;
        
        [Header("Options")] 
        [SerializeField] private bool storeInPlayerPrefs;
        [SerializeField] private bool resetToDefault;

        public T Value
        {
            get => storeInPlayerPrefs && SupportsPlayerPrefs ? GetFromPlayerPrefs(name, defaultValue) : value;
            set
            {
                if (storeInPlayerPrefs && SupportsPlayerPrefs)
                {
                    StoreInPlayerPrefs(name, value);
                }
                else
                {
                    this.value = value;
                }
            }
        }

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
            if (resetToDefault)
            {
                ResetToDefault();
            }
        }

        protected virtual void ResetToDefault()
        {
            value = defaultValue;
        }

        protected virtual bool SupportsPlayerPrefs => false;

        protected virtual T GetFromPlayerPrefs(string key, T defaultValue)
        {
            throw new NotSupportedException();
        }

        protected virtual void StoreInPlayerPrefs(string key, T actualValue)
        {
            throw new NotSupportedException();
        }
    }
}