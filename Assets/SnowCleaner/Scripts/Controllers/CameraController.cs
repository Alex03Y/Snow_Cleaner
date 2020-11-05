using SnowCleaner.Scripts.Core.Misc;
using UnityEngine;

namespace SnowCleaner.Scripts.Controllers
{
    public class CameraController : CachedBehaviour
    {
        [SerializeField] private Transform _targetTransform;

        [Space]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;

        private Transform _transform;
        private bool _gameWin;


        private void Awake()
        {
            // _transform = GetComponent<Transform>();
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            Transform.Value.rotation = Quaternion.Lerp(Transform.Value.rotation, _targetTransform.rotation, Time.deltaTime * _rotationSpeed);

            if (_gameWin) return;
            Transform.Value.position = Vector3.Lerp(Transform.Value.position, _targetTransform.position, Time.deltaTime * _moveSpeed);

        }

        public void GameWin()
        {
            _gameWin = true;
        }
    }
}