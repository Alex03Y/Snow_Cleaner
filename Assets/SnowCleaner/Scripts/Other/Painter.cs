using UnityEngine;

namespace SnowCleaner.Scripts.Other
{
    public class Painter : MonoBehaviour
    {
        [SerializeField] private RenderTexture _maskRenderTexture = null;
        [SerializeField] private Texture _maskTexture = null;
        [SerializeField] private Transform _transformPlane;

        [Space]
        [SerializeField] private Camera _cameraMain = null;
        [SerializeField] private float _brushSize = 2f;

        private Vector2 _maskResize;

        private void Start()
        {
            var localScale = _transformPlane.localScale;
            _maskResize = new Vector2(  _maskRenderTexture.width * localScale.x, _maskRenderTexture.height * localScale.z);
        }


        private void Update()
        {
            var click = Input.GetMouseButtonDown(0);
            var reset = Input.GetMouseButtonDown(1);

            if (reset) _maskRenderTexture.Release();
            
            if (!click) return;

            var rayFromCamera = _cameraMain.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayFromCamera, out var rayCastHit, 20))
            {
                RenderTexture.active = _maskRenderTexture;
                
            
                GL.PushMatrix();
                GL.LoadPixelMatrix(0, _maskResize.x, _maskResize.y, 0);

                var halfBrushSize = _brushSize * 0.5f;
                var rectX = rayCastHit.textureCoord.x * _maskResize.x - halfBrushSize;
            
                // var maskHeight = _maskRenderTexture.height;
                var rectY = _maskResize.y - rayCastHit.textureCoord.y * _maskResize.y - halfBrushSize;
         
                var rectTexture = new Rect(rectX, rectY, _brushSize, _brushSize);
                Graphics.DrawTexture(rectTexture, _maskTexture);
            
                GL.PopMatrix();
                RenderTexture.active = null;
            }
        }
    }
}
