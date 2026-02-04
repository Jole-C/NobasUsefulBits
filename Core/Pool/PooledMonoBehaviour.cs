using UnityEngine;
using Noba;

/// <summary>
/// MonoBehaviour derived class that is intended to be used on a pooled object.
/// Allows returning this object to the pool from a dedicated method (as opposed to destroying the object).
/// </summary>

namespace Noba.ObjectPool
{
    public class PooledMonoBehaviour : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] protected bool debug = false;

        //Called to return this object to the pool
        public virtual void ReturnToPool()
        {
            gameObject.SetActive(false);
            OnReturn();
        }

        //Called on spawned from the pool and returned to the pool.
        //Alternative methods to OnEnable and OnDisable, which may help to improve readability of the code.
        //They are optional but preferred to OnEnable/OnDisable as they specifically state what happens when the object is destroyed or spawned.
        protected virtual void OnReturn()
        {

        }

        public virtual void OnSpawn()
        {

        }

        //These methods are used when the object is "destroyed" (returned to the pool) or "spawned" (taken from the pool).
        //They are typically used to reset state.
        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }

        //Used on initial setup of this object, only once, when it first spawns.
        protected virtual void Awake()
        {
        }
        
        //Used on initial setup of this object to communicate with other objects, only once, when it first spawns.
        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
        {
        }
    }
}