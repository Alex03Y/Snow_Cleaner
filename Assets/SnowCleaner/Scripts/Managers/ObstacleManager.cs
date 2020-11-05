using System;
using System.Collections.Generic;
using SnowCleaner.Scripts.Core.ServiceLocator;
using SnowCleaner.Scripts.Entities;
using UnityEngine;

namespace SnowCleaner.Scripts.Managers
{
    public class ObstacleManager : MonoBehaviour, IService
    {
        public Type ServiceType => typeof(ObstacleManager);
        public event Action<CollisionArgs> OnCollision;
        
        [SerializeField] private Dictionary<int, Obstacle> _mapObstacles = new Dictionary<int, Obstacle>();

        public void RegisterObstacle(Obstacle obstacle)
        {
            var id = obstacle.Id;
            
            if(_mapObstacles.ContainsKey(id)) throw new Exception($"[ObstacleManager] This ID: {id} is already registered");
            
            _mapObstacles.Add(id, obstacle);
        }

        public void CollisionHappened(int idObstacle, int idEntering)
        {
            if (GetObstacleFromId(idObstacle, out var obstacle))
            {
                var args = new CollisionArgs(obstacle, idEntering);
                OnCollision?.Invoke(args);
            }
        }

        public bool GetObstacleFromId(int id, out Obstacle obstacle)
        {
            return _mapObstacles.TryGetValue(id, out obstacle);
        }
    }

    public class CollisionArgs
    {
        public readonly Obstacle IdObstacle;
        public readonly int IdEntering;

        public CollisionArgs(Obstacle idObstacle, int idEntering)
        {
            IdObstacle = idObstacle;
            IdEntering = idEntering;
        }
    }
}