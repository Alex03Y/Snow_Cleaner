using System;
using System.Collections;
using System.Runtime.InteropServices;
using NaughtyAttributes;
using SnowCleaner.Scripts.Core.ServiceLocator;
using SnowCleaner.Scripts.Managers;
using UnityEngine;

namespace SnowCleaner.Scripts.Controllers
{
    public class TrackMoveController : MonoBehaviour
    {
        [BoxGroup("References")] [SerializeField] private Transform _transform = null;
        
        [BoxGroup("Values")] [SerializeField] private float _speedMove = 50f;
        [BoxGroup("Values")] [SerializeField] private Vector2 _rangeOffsetToSide;
        [BoxGroup("Values")] [SerializeField] private float _koefOffsetToSide = 0.5f;

        [BoxGroup("AnimationCurves")] [SerializeField] private AnimationCurve _curveOffset;

        private InputManager _inputManager;
        private float _offsetToSideValue = 0f;

        private Vector3 _startPositionInput;
        private float _input;
        private float _koefInput = 0;
        private bool _inputActive;
        private float _prevDifferenceInput;

        private bool _gameLost;

        private void Awake()
        {
            _inputManager = ServiceLocator.Resolve<InputManager>();

            _inputManager.OnSingleTouchBegin += BeginTouchHandler;
            _inputManager.OnSingleTouchHold += HoldTouchHandler;
            _inputManager.OnSingleTouchEnded += EndTouchHandler;

            var cameraMain = Camera.main;
            if (cameraMain == null) throw new Exception("Main camera is Null");
            
            //After multiplying the input, its distance between the center of the screen and the edge from 0 to 1
            _koefInput = 1 / (cameraMain.pixelWidth * 0.5f); 
        }

        private void Update()
        {
            if (_gameLost) return;
            
           var moveDirection = _transform.forward;
           
           moveDirection.x = _offsetToSideValue;

           moveDirection = Vector3.ClampMagnitude(moveDirection, 1f) * (Time.deltaTime * _speedMove);
           _transform.position += moveDirection;
        }

        //Memorizing the center point
        private void BeginTouchHandler(Vector3 positionInput)
        {
            _startPositionInput = positionInput;
            _prevDifferenceInput = 0f;
            _inputActive = true;
        }

        /// <summary>
        /// The greater the distance between the center point and the new point, the faster the turn will be.
        /// The center point will change after changing direction.
        /// </summary>
        /// <param name="positionInput"></param>
        private void HoldTouchHandler(Vector3 positionInput)
        {
            var differenceInput = positionInput.x - _startPositionInput.x;
            differenceInput *= _koefInput;

            differenceInput = Mathf.Clamp(differenceInput, -1f, 1f);
            
            var stepIncrementOffset = _curveOffset.Evaluate(Mathf.Abs(differenceInput));
            stepIncrementOffset = Mathf.Clamp(stepIncrementOffset, 0, _rangeOffsetToSide.y);
            stepIncrementOffset *= Mathf.Sign(differenceInput) * _koefOffsetToSide * _speedMove;

            _offsetToSideValue += stepIncrementOffset;
            _offsetToSideValue = Mathf.Clamp(_offsetToSideValue, _rangeOffsetToSide.x, _rangeOffsetToSide.y);

            var differenceDelta = Mathf.Abs(differenceInput) - Mathf.Abs(_prevDifferenceInput);
            if (differenceDelta < -0.001f)
            {
                _startPositionInput = positionInput;
                _prevDifferenceInput = 0;
            }
            else
            {
                _prevDifferenceInput = differenceInput;
            }
        }

        private void EndTouchHandler(Vector3 positionInput)
        {
            _inputActive = false;
            StartCoroutine(LerpToZero());
        }

        //Automatic alignment
        private IEnumerator LerpToZero()
        {
            var f = _offsetToSideValue > 0 ? -1 : 1;
            while (Mathf.Abs(_offsetToSideValue) > 0.05f && !_inputActive)
            {
                _offsetToSideValue += f * Time.deltaTime * _speedMove * 0.2f;
                yield return null;
            }

            _offsetToSideValue = 0f;
        }

        public void GameLost()
        {
            _gameLost = true;
        }
    }
}