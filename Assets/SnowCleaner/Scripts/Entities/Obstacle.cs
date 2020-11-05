using SnowCleaner.Scripts.Core.Misc;
using SnowCleaner.Scripts.Core.ServiceLocator;
using SnowCleaner.Scripts.Managers;
using SnowCleaner.Scripts.Misc;
using UnityEngine;

namespace SnowCleaner.Scripts.Entities
{
    [RequireComponent(typeof(Trigger))]
    public class Obstacle : CachedBehaviour
    {
        [SerializeField] private Trigger _trigger;
        [SerializeField] private Collider _collider;

        private ObstacleManager _obstacleManager;

        public int Id => _collider.GetInstanceID();

        private void Awake()
        {
            if (_collider == null) _collider = GetComponent<Collider>();
            if (_trigger == null) _trigger = GetComponent<Trigger>();

            _obstacleManager = ServiceLocator.Resolve<ObstacleManager>();
            _obstacleManager.RegisterObstacle(this);
            
            _trigger.OnEnter += CollisionHandler;
        }

        private void CollisionHandler(int idCollider)
        {
            _obstacleManager.CollisionHappened(Id, idCollider);
        }
    }
}