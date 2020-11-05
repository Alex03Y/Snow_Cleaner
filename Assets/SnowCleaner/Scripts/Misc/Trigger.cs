using System;
using UnityEngine;

namespace SnowCleaner.Scripts.Misc
{
    [RequireComponent(typeof(Collider))]
    public class Trigger : MonoBehaviour
    {
        public event Action<int> OnEnter;
        public event Action<int> OnExit;
        
        private void OnTriggerEnter(Collider other)
        {
            var id = other.GetInstanceID();
            OnEnter?.Invoke(id);
        }
        
        private void OnTriggerExit(Collider other)
        {
            var id = other.GetInstanceID();
            OnExit?.Invoke(id);
        }
    }
}