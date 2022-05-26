using System;
using UnityEngine;

namespace Variables
{
    public class GenericVariable<T> : ScriptableObject
    {
        [SerializeField] private T value;
        [Header("Options")] 
        [SerializeField] private bool storeInPlayerPrefs;

        public T Value
        {
            get => storeInPlayerPrefs && SupportsPlayerPrefs ? GetFromPlayerPrefs(name, value) : value;
            set
            {
                if (storeInPlayerPrefs && SupportsPlayerPrefs)
                {
                    StoreInPlayerPrefs(name, value);
                }

                this.value = value;
            }
        }

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        public virtual bool SupportsPlayerPrefs => false;

        public virtual T GetFromPlayerPrefs(string key, T defaultValue)
        {
            throw new NotSupportedException();
        }

        public virtual void StoreInPlayerPrefs(string key, T actualValue)
        {
            throw new NotSupportedException();
        }
    }
}