using System;
using System.Collections.Generic;
using SnowCleaner.Scripts.Core.ServiceLocator;
using SnowCleaner.Scripts.Entities;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace SnowCleaner.Scripts.Managers
{
    public class SnowManager : MonoBehaviour, IService
    {
        public Type ServiceType => typeof(SnowManager);

        public event Action OcSnowClearStart;
        public event Action OcSnowClearEnd;

        [SerializeField] private RenderTexture _maskRenderTexture = null;
        [SerializeField] private Texture _maskTexture = null;
        [SerializeField] private float _brushSize = 20f;
        [SerializeField] private Texture _emptyTexture;
        
        private Dictionary<int, Snow> _snowMap = new Dictionary<int, Snow>();

        private int _currentIdSnow = 0;
        private Snow _currentSnow = null;

        private bool _isSnowCleaner = false;
        private RenderTexture _renderTexture;

        private void Awake()
        {
            _renderTexture = new RenderTexture(1024, 1024, 0);
            // _renderTexture.format = RenderTextureFormat.Default;
        }

        public void RegisterSnow(Snow snow)
        {
            var id = snow.Id;
            
            if (_snowMap.ContainsKey(id)) throw new Exception( $"[SnowManager] This Id {id} was being registered"); 
            
            _snowMap.Add(id, snow);
        }

        public void CleanSnow(RaycastHit hit)
        {
            var id = hit.collider.GetInstanceID();
            
            if (id!= _currentIdSnow)
            {
                if (!_snowMap.TryGetValue(id, out var snow))
                {
                    if (_isSnowCleaner)
                    {
                        OcSnowClearEnd?.Invoke();
                        _isSnowCleaner = false;
                        _currentIdSnow = -1;
                        _currentSnow.EndClean(_emptyTexture);
                    }
                    return;
                }

                _currentIdSnow = id; 
                _currentSnow = snow;
                
                _currentSnow.Initialize(_renderTexture, _maskTexture, _brushSize);
                OcSnowClearStart?.Invoke();
                _isSnowCleaner = true;
            }
            
            _currentSnow.Clean(hit);
        }

        private void OnDestroy()
        {
            Destroy(_renderTexture);
        }
    }
}