using System;
using System.Collections;
using System.Collections.Generic;
using SnowCleaner.Scripts.Misc;
using UnityEngine;

namespace SnowCleaner.Scripts.Entities
{
    public class Leader : MonoBehaviour
    {
        public event Action OnStartMove;

        [SerializeField] private Transform _transform;
        [SerializeField] private float _delaySpawnPoints;
        [SerializeField] private VelocityMeter _velocityMeter;

        private bool _canMove;

        public Queue<Vector3> _queueWayPoints = new Queue<Vector3>();

        private void Start()
        {
            StartCoroutine(SpawnPoint());
            OnStartMove?.Invoke();
        }

        private IEnumerator SpawnPoint()
        {
            while (true)
            {
                yield return new WaitForSeconds(_delaySpawnPoints);

                if (!_canMove) continue;
                _queueWayPoints.Enqueue(_transform.position);
            }
            
            
            yield return null;
        }

        private void Update()
        {
            _canMove = _velocityMeter.Magnitude > 0.01f;
        }

        public bool GetPoint(out Vector3 targetPoint, out Vector3 direction)
        {

            if (_queueWayPoints.Count < 2)
            {
                targetPoint = Vector3.one;
                direction = targetPoint;
                return false;
            }
            
            targetPoint = _queueWayPoints.Dequeue();
            direction = (_queueWayPoints.Peek() - targetPoint).normalized;
            
            return true;
        }
    }
}