using System;
using System.Collections.Generic;

/// <summary>
/// Basic Service Locator class.
/// </summary>

namespace Noba.Service
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> services = new();

        public static void RegisterService<T>(T service)
        {
            services[typeof(T)] = service;
        }

        public static bool TryGetService<T>(out T service)
        {
            if (services.TryGetValue(typeof(T), out object obj) && obj is T typed)
            {
                service = typed;
                return true;
            }

            service = default;
            return false;
        }

        public static void UnregisterService<T>()
        {
            services.Remove(typeof(T));
        }
    }
}