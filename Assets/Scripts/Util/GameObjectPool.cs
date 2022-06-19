using UnityEngine;
using UnityEngine.Pool;

namespace Util
{
    [CreateAssetMenu]
    public class GameObjectPool : ScriptableObject
    {
        [Header("Prefab")]
        public GameObject prefab;
        
        [Header("Pool Capacity")]
        [Tooltip("The maximum number of objects to keep in pool")]
        [Min(1)] public int maximumSize = 10;

        [Tooltip("The number of instances to create at initialization time")] [Min(0)]
        public int initialSize;

        private ObjectPool<GameObject> pool;

        private void OnEnable()
        {
            pool = new ObjectPool<GameObject>(() => Instantiate(prefab),
                obj => obj.SetActive(true),
                obj => obj.SetActive(false),
                Destroy,
                defaultCapacity: initialSize,
                maxSize: maximumSize
            );
        }

        private void OnDisable()
        {
            pool.Dispose();
            pool = null;
        }

        public GameObject Get()
        {
            return pool.Get();
        }

        public void Release(GameObject obj)
        {
            pool.Release(obj);
        }
    }
}