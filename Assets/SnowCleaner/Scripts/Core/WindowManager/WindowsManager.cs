using System;
using System.Collections.Generic;
using NaughtyAttributes;
using SnowCleaner.Scripts.Core.Misc;
using SnowCleaner.Scripts.Core.ServiceLocator;
using UnityEditor;
using UnityEngine;

namespace SnowCleaner.Scripts.Core.WindowManager
{
    public sealed class WindowsManager : CachedBehaviour, IService
    {
        Type IService.ServiceType { get; } = typeof(WindowsManager);
        public IWindowController OpenedWindow { get; private set; }

        [SerializeField, ReorderableList] private WindowView[] _windows;
        
        private readonly Dictionary<int, WindowView> _windowsMap = new Dictionary<int, WindowView>();

        public event Action<bool> OnOpenWindow; 

        private void Awake()
        {
            foreach (var view in _windows)
            {
                _windowsMap.Add(view.WindowId, view);
                view.gameObject.SetActive(false);
                view.OnPopupClosed += AnyWindowClosed;
            }

            _windows = null;
        }

        public void RequestShowWindow(IWindowController controller)
        {
            if (OpenedWindow != null && OpenedWindow.View.IsOpened)
            {
                var currentPopup = OpenedWindow;
                currentPopup.View.Close();
                currentPopup.View = null;
            }

            OnOpenWindow?.Invoke(true);

            _windowsMap.TryGetValue(controller.WindowId, out var view);
            if (ReferenceEquals(view, null)) throw new NullReferenceException("Can't find popup with id: " + controller.WindowId);
            
            view.Controller = controller;
            controller.View = view;

            OpenedWindow = controller;
            view.Transform.Value.SetAsLastSibling();
            view.Open();
        }

        public void CloseWindow()
        {
            if (OpenedWindow != null && OpenedWindow.View.IsOpened)
            {
                var currentPopup = OpenedWindow;
                currentPopup.View.Close();
                currentPopup.View = null;
                OpenedWindow = null;
            }
        }

        private void AnyWindowClosed(IWindowController controller)
        {
            OnOpenWindow?.Invoke(false);
        }

        private void OnDestroy()
        {
            // foreach (var view in _windows)
            // {
            //     view.OnPopupClosed -= AnyWindowClosed;
            // }
        }

#if UNITY_EDITOR
        [Button("Collect All Windows")]
        private void CollectAllWindows()
        {
            _windows = GetComponentsInChildren<WindowView>(true);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}