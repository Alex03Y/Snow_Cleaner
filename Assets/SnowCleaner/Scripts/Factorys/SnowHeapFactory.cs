using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using SnowCleaner.Scripts.Core.Factory;
using SnowCleaner.Scripts.Core.Pool;
using SnowCleaner.Scripts.Core.ServiceLocator;
using SnowCleaner.Scripts.Entities;
using SnowCleaner.Scripts.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnowCleaner.Scripts.Factorys
{
    public class SnowHeapFactory : MonoBehaviour, IFactory<SnowHeap,SnowHeapArgs>
    {
        [SerializeField, ReorderableList] private List<SnowHeap> _snowHeapPrefabs;
        [SerializeField] private int _poolSize = 20;

        private Dictionary<SnowHeapType, SnowHeap> _snowHeapMap;
        private PoolManager _poolManager;
        
        public void Initialize()
        {
            // Debug.Log("Initialize");
            _poolManager = ServiceLocator.Resolve<PoolManager>();
            _snowHeapMap = _snowHeapPrefabs.ToDictionary(x => x.Type);
            _snowHeapPrefabs.ForEach(x => _poolManager.CreatePool(x.gameObject, _poolSize));

        }
        public SnowHeap Create(SnowHeapArgs args)
        {
            // Debug.Log("Create");

            var randomHeapNumber = Random.Range(100, 104);
            var type = (SnowHeapType) randomHeapNumber;

            var heap = _snowHeapMap[type].gameObject;
            var rotation = Random.rotation;
            var poolObject = _poolManager.InstantiateFromPool(heap, args.StartPoint.position, rotation);

            var snowHeap = poolObject.ObjectLocator.Resolve<SnowHeap>();
            snowHeap.Initialize(args);
            
            
            return null;
        }

        public Type ServiceType => typeof(IFactory<SnowHeap,SnowHeapArgs>);

        
    }

    public class SnowHeapArgs
    {
        public readonly Vector3 EndPoint;
        public readonly float Duration;
        public readonly Transform StartPoint;
        public readonly Transform ParentForHeapAfterAnimation;

        public SnowHeapArgs(Vector3 endPoint, float duration, Transform startPoint, Transform parentForHeapAfterAnimation)
        {
            EndPoint = endPoint;
            Duration = duration;
            StartPoint = startPoint;
            ParentForHeapAfterAnimation = parentForHeapAfterAnimation;
        }
    }
}