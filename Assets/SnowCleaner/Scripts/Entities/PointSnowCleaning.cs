using System.Collections;
using SnowCleaner.Scripts.Core.ServiceLocator;
using SnowCleaner.Scripts.Managers;
using UnityEngine;

namespace SnowCleaner.Scripts.Entities
{
    public class PointSnowCleaning : MonoBehaviour
    {
        [SerializeField] private Transform _pointForRayCast;
        [SerializeField] private float _delay = 0.01f;
        private SnowManager _snowManager;

        private bool isSnowCleaning;


        private void Awake()
        {
            _snowManager = ServiceLocator.Resolve<SnowManager>(); 
        }

        private IEnumerator Start()
        {
            while (true)
            {
                var ray = new Ray(_pointForRayCast.position,_pointForRayCast.up * -1f);
                if (Physics.Raycast(ray, out var rayCastHit, 5))
                {
                    _snowManager.CleanSnow(rayCastHit);
                }

                yield return new WaitForSeconds(_delay);
            }

            yield return null;
        }

        // For visual debug
        private void OnDrawGizmos()
        {
            var color = Color.red;
            Gizmos.color = color;
            Gizmos.DrawRay(_pointForRayCast.position, _pointForRayCast.up * -1f);
        }
    }
}