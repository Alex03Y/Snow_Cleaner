using System.Collections.Generic;
using System.Linq;
using SnowCleaner.Scripts.Core.Factory;
using UnityEngine;

namespace SnowCleaner.Scripts.Core.ServiceLocator
{
    public class ServiceRegister : MonoBehaviour
    {
        public bool IncludeInactive = true;
        private void Awake()
        {
            List<IService> findServices = null;
            
            findServices = new List<IService>(GetComponentsInChildren<IService>(IncludeInactive));
            
            findServices.ForEach(ServiceLocator.Register);
            var factories = findServices.OfType<IFactory>();
            foreach (var factory in factories)
                factory.Initialize();
        }

    }
}