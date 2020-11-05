using SnowCleaner.Scripts.Core.Misc;
using SnowCleaner.Scripts.Misc;
using UnityEngine;

namespace SnowCleaner.Scripts.Controllers
{
    public class CarRotateController : CachedBehaviour
    {
        [Header("Сar turn")]
        [SerializeField] private Transform _transformVisual;
        [SerializeField] private float _koefLerpTurn;
        [SerializeField] private VelocityMeter _velocityMeter;
        
        [Header("Car tilt")] 
        [SerializeField] private float _koefLerpTilt = 0.5f;
        [SerializeField] private float _powerTilt = 1f;
        [SerializeField] private float _maxAngleTilt = 10f;

        private Vector3 _prevDirection;

        // private float _rotation;

        private void Start()
        {
            _prevDirection = _transformVisual.forward;
        }

        private void Update()
        {
            if(_velocityMeter.Magnitude < 0.001f) return;
            
            //Rotate around vertical direction
            var currentDirection = Transform.Value.forward;
            var normalVelocity = _velocityMeter.Velocity.normalized;
            var nextDirection = Vector3.Lerp(currentDirection, normalVelocity, _koefLerpTurn * Time.deltaTime);
            
            if(nextDirection != Vector3.zero) Transform.Value.forward = nextDirection;

            //Rotate around forward direction
            var delta = (nextDirection - _prevDirection).x;
            delta *= _powerTilt;
            delta = Mathf.Clamp(delta, -_maxAngleTilt, _maxAngleTilt);
            var nextRotation = Quaternion.Euler(0f, 0f,delta);
            _transformVisual.localRotation =
                Quaternion.Lerp(_transformVisual.localRotation, nextRotation, _koefLerpTilt * Time.deltaTime);
            
            _prevDirection = nextDirection;

        }
    }
}