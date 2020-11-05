using System;
using System.Collections.Generic;
using NaughtyAttributes;
using SnowCleaner.Scripts.Controllers;
using SnowCleaner.Scripts.Core.ServiceLocator;
using SnowCleaner.Scripts.Enums;
using SnowCleaner.Scripts.Managers;
using UnityEngine;

namespace SnowCleaner.Scripts.Entities
{
    public class LevelEndFinalization : MonoBehaviour
    {
        [SerializeField] private Collider _trackCollider;
        [SerializeField] private TrackMoveController _trackMoveController;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private SnowHeapSpawnController spawnController;
        [SerializeField, ReorderableList] private List<Follower> _followers;

        private LevelProgressController _levelProgressController;
        private InputManager _inputManager;

        private void Awake()
        {
            _levelProgressController =ServiceLocator.Resolve<LevelProgressController>();
            _inputManager = ServiceLocator.Resolve<InputManager>();
        }

        private void Start()
        {
            _levelProgressController.RegisterPlayer(_trackCollider.GetInstanceID());
            _levelProgressController.OnGameOver += GameOver;
        }

        private void GameOver(GameStatus status)
        {
            switch (status)
            {
                case GameStatus.Lost:
                    _trackMoveController.GameLost();
                    _followers.ForEach(x => x.GameOver());
                    spawnController.GameLost();
                    break;
                case GameStatus.Win:
                    _inputManager.DisableInput();
                    _cameraController.GameWin();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}