using SnowCleaner.Scripts.Core.ServiceLocator;
using SnowCleaner.Scripts.Core.Timer;
using SnowCleaner.Scripts.Managers;
using UnityEngine;

namespace SnowCleaner.Scripts.Entities
{
    [RequireComponent(typeof(Collider))]
    public class Snow : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _transformPlane;
        [SerializeField] private Renderer _renderer;

        public int Id => _collider.GetInstanceID();

        private RenderTexture _maskRenderTexture;
        private Texture _maskTexture;
        private SnowManager _snowManager;
        private Vector2 _maskResize;
        private float _brushSize = 20f;

        
        private static readonly int Mask = Shader.PropertyToID("Texture2D_Mask");

        private void Awake()
        {
            _snowManager = ServiceLocator.Resolve<SnowManager>();
            
            _snowManager.RegisterSnow(this);
        }

        //Setup mask size and set render texture;
        public void Initialize(RenderTexture texture, Texture mask, float brushSize)
        {
            _maskRenderTexture = texture;
            _brushSize = brushSize;
            _maskTexture = mask;
            
            _maskRenderTexture.Release();
            
            var propertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetTexture(Mask, _maskRenderTexture);
            _renderer.SetPropertyBlock(propertyBlock);
            
            var localScale = _transformPlane.localScale;
            _maskResize = new Vector2(  _maskRenderTexture.width * localScale.x, _maskRenderTexture.height * localScale.z);
        }

        //Draw to the RenderTexture that the shader uses
        public void Clean(RaycastHit hit)
        {
            RenderTexture.active = _maskRenderTexture;

            GL.PushMatrix();
            
            GL.LoadPixelMatrix(0, _maskResize.x, _maskResize.y, 0);

            var halfBrushSize = _brushSize * 0.5f;
            var rectX = hit.textureCoord.x * _maskResize.x - halfBrushSize;

            var rectY = (_maskResize.y - hit.textureCoord.y * _maskResize.y) - halfBrushSize;

            var rectTexture = new Rect(rectX, rectY, _brushSize, _brushSize);
            Graphics.DrawTexture(rectTexture, _maskTexture);

            GL.PopMatrix();
            RenderTexture.active = null;
        }

        public void EndClean(Texture textureEmpty)
        {
            
            // var propertyBlock = new MaterialPropertyBlock();
            // _renderer.GetPropertyBlock(propertyBlock);
            
            // Timer.Register(5f, () =>
            // {
            //     propertyBlock.SetTexture(Mask, textureEmpty);
            //     _renderer.SetPropertyBlock(propertyBlock);
            // });
            
        }
    }
}