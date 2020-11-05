using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnowCleaner.Scripts.Core.ServiceLocator
{
    
    /// <summary>
    /// Alternative of Zenject.
    /// </summary>
    public class ServiceLocator : MonoBehaviour
    {
        private static readonly Dictionary<Type, IService> _serviceMap = new Dictionary<Type, IService>();


        public static void Register(IService service)
        {
            if (_serviceMap.ContainsKey(service.ServiceType))
                throw new Exception($"[ServiceLocator] Service {service.ServiceType.Name} already registered.");

            _serviceMap.Add(service.ServiceType, service);
        }
        
        public static TRegister Resolve<TRegister>() where TRegister : class
        {
            var serviceType = typeof(TRegister);

            if (_serviceMap.ContainsKey(serviceType)) return (TRegister) _serviceMap[serviceType];
            throw new Exception($"[ServiceLocator] Service {serviceType.FullName} was not being register.");
        }
        
        private void OnDestroy()
        {
            _serviceMap.Clear();
        }
    }
}