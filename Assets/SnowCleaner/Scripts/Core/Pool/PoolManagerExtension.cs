using UnityEngine;

namespace SnowCleaner.Scripts.Core.Pool
{
    public static class PoolManagerExtension
    {
        public static PoolObject InstantiateFromPool(this PoolManager poolManager, GameObject prefab,
            Vector3 position)
        {
            var poolObject = poolManager.InstantiateFromPool(prefab);
            poolObject.Transform.position = position;
            return poolObject;
        }

        public static PoolObject InstantiateFromPool(this PoolManager poolManager, GameObject prefab,
            Vector3 position, Quaternion rotation)
        {
            var poolObject = poolManager.InstantiateFromPool(prefab, position);
            poolObject.Transform.rotation = rotation;
            return poolObject;
        }
    }
}