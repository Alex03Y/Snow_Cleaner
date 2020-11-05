using UnityEngine;

namespace SnowCleaner.Scripts.Misc
{
    public class VelocityMeter : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        
        private Vector3 _lastPosition;
        private Vector3 _velocity;
        private float _magnitude;
        
        private bool _isZero;

        public Vector3 Velocity => _velocity;
        public float Magnitude => _magnitude;


        private void Awake()
        {
            if (_transform == null) _transform = GetComponent<Transform>();
            _lastPosition = _transform.position;
        }

        private void Update()
        {
            var currentPosition = _transform.position;
            var velocity = currentPosition - _lastPosition;
            var magnitude = velocity.magnitude;

            if (magnitude < 0.00001f && !_isZero)
            {
                _isZero = true;
                magnitude = 0f;
                _velocity = Vector3.zero;
                return;
            }

            _velocity = velocity;
            _magnitude = magnitude;
            _lastPosition = currentPosition;
            _isZero = false;


        }
    }
}