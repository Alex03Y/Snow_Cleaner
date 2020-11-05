using System;
using DG.Tweening;
using SnowCleaner.Scripts.Core.WindowManager;
using SnowCleaner.Scripts.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SnowCleaner.Scripts.GUI
{
    public class GameOverPopupView : BaseWindowView<GameOverPopup>
    {
        public override int WindowId => (int) WindowType.GameOver;

        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _textField;
        [SerializeField] private string _winText;
        [SerializeField] private string _lostText;
        

        private GameStatus _status;

        public override void Open()
        {
            Transform.Value.localScale = Vector3.zero;
            Debug.Log("Opened");
            gameObject.SetActive(true);
            Initialize();
            Transform.Value.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
            IsOpened = true;
        }

        protected override void Initialize()
        {
            Debug.Log("Initialize");
            _restartButton.onClick.AddListener(ClickOnRestart);
            _status = ConcreteController._status;
            FillTextField();
        }

        private void FillTextField()
        {
            switch (_status)
            {
                case GameStatus.Lost:
                    _textField.text = _lostText;
                    break;
                case GameStatus.Win:
                    _textField.text = _winText;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ClickOnRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        protected override void Dispose()
        {
            _restartButton.onClick.RemoveAllListeners();
        }
    }
}