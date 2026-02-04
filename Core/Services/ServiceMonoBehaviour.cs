using UnityEngine;

/// <summary>
/// Used by the Service Locator but not necessary as it is a templated class.
/// Base class for services like the object pool manager.
/// </summary>

namespace Noba.Service
{
    public class ServiceMonoBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
        }
    }
}