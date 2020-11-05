using System;
using SnowCleaner.Scripts.Core.Misc;
using Vector3 = UnityEngine.Vector3;

namespace SnowCleaner.Scripts.Core.WindowManager
{
    public abstract class WindowView : CachedBehaviour
    {
        public abstract int WindowId { get; }
        public virtual IWindowController Controller { get; set; }
        public virtual bool IsOpened { get; protected set; }

        public event Action<IWindowController> OnPopupClosed;
        
        public virtual void Open()
        {
            IsOpened = true;
            gameObject.SetActive(true);
            Transform.Value.localScale = Vector3.zero;
        }

        public virtual void Close()
        {
            
            IsOpened = false;
            gameObject.SetActive(false);
            Dispose();
            OnPopupClosed?.Invoke(Controller);
            
            Controller = null;
        }

        protected abstract void Initialize();
        protected abstract void Dispose();
    }

    public abstract class BaseWindowView<T> : WindowView where T : class, IWindowController
    {
        public T ConcreteController => (T) Controller;
    }
}
