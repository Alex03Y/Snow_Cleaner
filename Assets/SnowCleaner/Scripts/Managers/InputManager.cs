using System;
using SnowCleaner.Scripts.Core.ServiceLocator;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SnowCleaner.Scripts.Managers
{
    public class InputManager : MonoBehaviour, IService
    {
        public Type ServiceType => typeof(InputManager);

        [SerializeField] private EventSystem _eventSystem;

        public event Action<Vector3> OnSingleTouchBegin;
        public event Action<Vector3> OnSingleTouchHold;
        public event Action<Vector3> OnSingleTouchEnded;

        private float _beginSingleTouchTime;
        private bool _activeInput = true;

        private void Update()
        {
            if (!_activeInput) return;
            SingleTouchProcessing();
        }

        private void SingleTouchProcessing()
        {
#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
            {
                _beginSingleTouchTime = Time.time;
                OnSingleTouchBegin?.Invoke(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                OnSingleTouchHold?.Invoke(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // if(_beginSingleTouchTime > 0f && Time.time - _beginSingleTouchTime > 0.2f) return;
                _beginSingleTouchTime = 0f;
                
                OnSingleTouchEnded?.Invoke(Input.mousePosition);
            }
#elif UNITY_ANDROID || UNITY_IOS

            if (Input.touchCount != 1) return;
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _beginSingleTouchTime = Time.time;
                OnSingleTouchBegin?.Invoke(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
                OnSingleTouchHold?.Invoke(touch.position);
            else if (touch.phase == TouchPhase.Ended)
            {
                // if(_beginSingleTouchTime > 0f && Time.time - _beginSingleTouchTime > 0.1f) return;
                _beginSingleTouchTime = 0f;
                
                OnSingleTouchEnded?.Invoke(touch.position);
            }
#endif
        }

        public void DisableInput()
        {
            _activeInput = false;
        }

    }
}