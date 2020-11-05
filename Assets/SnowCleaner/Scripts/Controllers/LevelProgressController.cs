using System;
using SnowCleaner.Scripts.Core.ServiceLocator;
using SnowCleaner.Scripts.Core.Timer;
using SnowCleaner.Scripts.Core.WindowManager;
using SnowCleaner.Scripts.Enums;
using SnowCleaner.Scripts.GUI;
using SnowCleaner.Scripts.Managers;
using SnowCleaner.Scripts.Misc;
using UnityEngine;

namespace SnowCleaner.Scripts.Controllers
{
    public class LevelProgressController : MonoBehaviour, IService
    {
        public Type ServiceType => typeof(LevelProgressController);

        public event Action<GameStatus> OnGameOver;
        
        [SerializeField] private Trigger _triggerFinish;
        private GameStatus _status;
        public GameStatus Status => _status;

        private ObstacleManager _obstacleManager;
        private WindowsManager _windowsManager;

        private int _playerID = -1;

        private void Awake()
        {
            _obstacleManager = ServiceLocator.Resolve<ObstacleManager>();
            _windowsManager = ServiceLocator.Resolve<WindowsManager>();
            _obstacleManager.OnCollision += GameLost;
            _triggerFinish.OnEnter += GameWin;
            _status = GameStatus.Continue;
        }

        public void RegisterPlayer(int id)
        {
            if (_playerID > 0 && _playerID != id)
            {
                Debug.LogException(new Exception($"[LevelController] Player is already registered by id: {_playerID}"));
                return;
            }

            _playerID = id;
        }

        private void GameLost(CollisionArgs args)
        {
            if (args.IdEntering == _playerID)
            {
                _status = GameStatus.Lost;
                OpenPopup();
            }
        }

        private void GameWin(int id)
        {
            if (id == _playerID)
            {
                _status = GameStatus.Win;
                OpenPopup();
            }
        }

        private void OpenPopup()
        {
            OnGameOver?.Invoke(_status);
            _obstacleManager.OnCollision -= GameLost;
            _triggerFinish.OnEnter -= GameWin;

            Timer.Register(1.5f, () =>
            {
                var controller = new GameOverPopup(_status);
                Debug.Log(_windowsManager == null);
                _windowsManager.RequestShowWindow(controller);
            });
        }

    }
}