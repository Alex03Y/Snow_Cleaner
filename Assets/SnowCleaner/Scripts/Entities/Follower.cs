using System.Collections;
using SnowCleaner.Scripts.Core.Misc;
using UnityEngine;

namespace SnowCleaner.Scripts.Entities
{
    public class Follower : CachedBehaviour
    {
        [SerializeField] private float _speedMove;
        [SerializeField] private Leader _leader;

        private bool _isMove;

        private Vector3 _targetPoint;
        private Vector3 _direction;

        private bool _gameOver;

        private Coroutine _follow;
        private Coroutine _getPoint;
        
        private void Awake()
        {
            _leader.OnStartMove += () => StartCoroutine(GetNextPoint());
        }

        private IEnumerator Following()
        {

            while ((_targetPoint - Transform.Value.position).magnitude > 0.1f && !_gameOver)
            {
                Transform.Value.position += Vector3.ClampMagnitude(_direction, 1f) * (Time.deltaTime * _speedMove);
                yield return null;
            }
            
            _isMove = false;

            _getPoint = StartCoroutine(GetNextPoint());

            yield return null;
        }

        private IEnumerator GetNextPoint()
        {
            while (true)
            {
                _isMove = _leader.GetPoint(out var point, out var dir);
                if (_isMove)
                {
                    _targetPoint = point;
                    _direction = dir;
                    break;
                }
                
                yield return new WaitForSeconds(0.1f);
            }

            _follow = StartCoroutine(Following());

            yield return null;
        }

        public void GameOver()
        {
            if (_follow != null) StopCoroutine(_follow);
            if (_getPoint != null) StopCoroutine(_getPoint);
            _gameOver = false;
        }
    }
}