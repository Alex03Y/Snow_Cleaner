using DG.Tweening;
using SnowCleaner.Scripts.Core.Misc;
using SnowCleaner.Scripts.Core.Pool;
using SnowCleaner.Scripts.Enums;
using SnowCleaner.Scripts.Factorys;
using UnityEngine;

namespace SnowCleaner.Scripts.Entities
{
    public class SnowHeap : CachedBehaviour, IPoolObject
    {
        [SerializeField] private SnowHeapType _type;

        private PoolObject _poolObject;

        private Transform _parent;
        private Vector3 _endPoint;
        
        public PoolObject PoolObject => _poolObject;
        public SnowHeapType Type => _type;


        //Start animation shoveling snow heap
        public void Initialize(SnowHeapArgs args)
        {
            Transform.Value.parent = args.StartPoint;
            Transform.Value.DOScale(Vector3.one, args.Duration * 0.3f).OnComplete((() =>
            {
                Transform.Value.DOScale(Vector3.one * 2, args.Duration * 0.7f);
                Transform.Value.DOLocalMove(args.EndPoint, args.Duration).OnComplete((() =>
                {
                    Transform.Value.parent = args.ParentForHeapAfterAnimation;
                }));
            }));
        }
        
        public void PostAwake(PoolObject poolObject)
        {
            _poolObject = poolObject;
            _poolObject.ObjectLocator.Register(this);
            Transform.Value.localScale = Vector3.zero;
        }

        public void OnReuseObject(PoolObject poolObject)
        {
        }

        public void OnDisposeObject(PoolObject poolObject)
        {
            Transform.Value.localScale = Vector3.zero;
        }
    }
}