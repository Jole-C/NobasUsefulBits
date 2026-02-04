using System.Collections.Generic;
using UnityEngine;
using Noba.Service;

/// <summary>
/// An Object Pooling Manager.
/// Supports multiple pools for multiple objects, dynamically created and assigned.
/// </summary>
/// 

namespace Noba.ObjectPool
{
    public class ObjectPoolManager : ServiceMonoBehaviour
    {
        Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();
        GameObject deferredObject = null;

        public bool TryAddPool(GameObject prefab)
        {
            if (pools.ContainsKey(prefab))
            {
                return false;
            }

            AddPool(prefab);

            return true;
        }

        public GameObject SpawnObject(GameObject prefab, Vector3 location = default, Quaternion rotation = default, Transform parent = null)
        {
            if (deferredObject)
            {
                Debug.LogError("Trying to spawn a new object when the deferred one is not finalised.");
                return null;
            }

            TryGetPool(prefab, out ObjectPool pool);

            return pool.SpawnObject(location, rotation, parent);
        }

        public GameObject SpawnObjectDeferred(GameObject prefab, Vector3 location = default, Quaternion rotation = default, Transform parent = null)
        {
            if (deferredObject)
            {
                Debug.LogError("Trying to spawn a new object when the deferred one is not finalised.");
                return null;
            }

            TryGetPool(prefab, out ObjectPool pool);

            deferredObject = pool.SpawnObjectDeferred(location, rotation, parent);

            return deferredObject;
        }

        public void FinishSpawning()
        {
            if (!deferredObject)
            {
                Debug.LogError("Trying to finish spawning a deferred object that doesn't exist.");
            }

            deferredObject.SetActive(true);
            deferredObject = null;
        }

        protected override void Awake()
        {
            ServiceLocator.RegisterService<ObjectPoolManager>(this);
        }

        void OnDestroy()
        {
            ServiceLocator.UnregisterService<ObjectPoolManager>();
        }

        ObjectPool AddPool(GameObject prefab)
        {
            ObjectPool pool = new ObjectPool(prefab);

            pools.Add(prefab, pool);

            return pool;
        }

        bool TryGetPool(GameObject prefab, out ObjectPool pool)
        {
            bool exists = pools.TryGetValue(prefab, out pool);

            if (!exists)
            {
                pool = AddPool(prefab);
            }

            return exists;
        }
    }
}
