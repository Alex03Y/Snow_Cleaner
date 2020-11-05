using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SnowCleaner.Scripts.Other.Preloader
{
    public class LoaderGameScene : MonoBehaviour
    {
        [SerializeField] private Button _btnStart;

        private void Start()
        {
            _btnStart.onClick.AddListener(LoadGameScene);
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }

        private void OnDestroy()
        {
            _btnStart.onClick.RemoveAllListeners();
        }
    }
}