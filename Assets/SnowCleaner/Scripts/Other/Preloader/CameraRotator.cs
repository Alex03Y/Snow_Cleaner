using SnowCleaner.Scripts.Core.Misc;
using UnityEngine;

namespace SnowCleaner.Scripts.Other.Preloader
{
    public class CameraRotator : CachedBehaviour
    {
        [SerializeField] private float _speedRotate;

        private void Update()
        {
            var angle = _speedRotate * Time.deltaTime;
            Transform.Value.rotation *= Quaternion.Euler(0f, angle, 0f);
        }
    }
}