using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Object Pool assigned to a specific game object.
/// Supports deferring object spawns by returning them deactivated, allowing the object to be modified before it is enabled.
/// This prevents issues like an object appearing at its death position for a frame before being moved.
/// </summary>
/// 

namespace Noba.ObjectPool
{
    public class ObjectPool
    {
        GameObject prefabClass;
        List<GameObject> prefabInstances = new List<GameObject>();

        public ObjectPool(GameObject prefab)
        {
            prefabClass = prefab;
        }

        GameObject TryGetInstance(Vector3 location, Quaternion rotation, Transform parent, bool activate = true)
        {
            GameObject returnedPrefab = null;

            foreach (GameObject instance in prefabInstances)
            {
                if (!instance.activeInHierarchy)
                {
                    returnedPrefab = instance;

                    returnedPrefab.transform.parent = parent;
                    returnedPrefab.transform.position = location;
                    returnedPrefab.transform.rotation = rotation;

                    returnedPrefab.SetActive(activate);

                    PooledMonoBehaviour pooledMonoBehaviour = instance.GetComponent<PooledMonoBehaviour>();

                    if (pooledMonoBehaviour)
                    {
                        pooledMonoBehaviour.OnSpawn();
                    }

                    break;
                }
            }

            if (!returnedPrefab)
            {
                returnedPrefab = GameObject.Instantiate(prefabClass, location, rotation, parent);
                prefabInstances.Add(returnedPrefab);
                returnedPrefab.SetActive(activate);
            }

            return returnedPrefab;
        }

        public GameObject SpawnObject(Vector3 location, Quaternion rotation, Transform parent)
        {
            GameObject instance = TryGetInstance(location, rotation, parent);

            return instance;
        }

        public GameObject SpawnObjectDeferred(Vector3 location, Quaternion rotation, Transform parent)
        {
            GameObject instance = TryGetInstance(location, rotation, parent, false);

            return instance;
        }
    }
}