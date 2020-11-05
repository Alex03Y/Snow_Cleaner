using System.Collections;
using SnowCleaner.Scripts.Core.Factory;
using SnowCleaner.Scripts.Core.Misc;
using SnowCleaner.Scripts.Core.ServiceLocator;
using SnowCleaner.Scripts.Entities;
using SnowCleaner.Scripts.Factorys;
using SnowCleaner.Scripts.Managers;
using UnityEngine;

namespace SnowCleaner.Scripts.Controllers
{
    public class SnowHeapSpawnController : CachedBehaviour
    {
        [Header("Parameters")] 
        [SerializeField] private float _durationAnimation;
        [SerializeField] private float _delaySpawnHip;


        [Header("References")] 
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _pointRight;
        [SerializeField] private Transform _pointLeft;
        [SerializeField] private Transform _parentForHeap;

        private bool _spawnHeaps;
        private Coroutine _activeCoroutine;
        
        private IFactory<SnowHeap, SnowHeapArgs> _factory;
        private void Start()
        {
            _factory = ServiceLocator.Resolve<IFactory<SnowHeap,SnowHeapArgs>>();

            var snowManager = ServiceLocator.Resolve<SnowManager>();
            snowManager.OcSnowClearStart += SnowStart;
            snowManager.OcSnowClearEnd += SnowEnd;
        }

        private void SnowStart()
        {
            _spawnHeaps = true;
            _activeCoroutine = StartCoroutine(SpawnHeaps());
        }

        //Spawns two heaps for the two sides
        private IEnumerator SpawnHeaps()
        {
            while (_spawnHeaps)
            {
                var argsForLeft = new SnowHeapArgs(_pointLeft.localPosition, _durationAnimation, _startPoint, _parentForHeap);
                var argsForRight = new SnowHeapArgs(_pointRight.localPosition, _durationAnimation, _startPoint, _parentForHeap);
                _factory.Create(argsForLeft);
                _factory.Create(argsForRight);
                
                yield return  new WaitForSeconds(_delaySpawnHip);
            }
            yield return null;
        }

        private void SnowEnd()
        {
            _spawnHeaps = false;
            if (!ReferenceEquals(_activeCoroutine, null))
            {
                StopCoroutine(_activeCoroutine);
                _activeCoroutine = null;
            }
        }

        public void GameLost()
        {
            _spawnHeaps = false;
        }
    }
}