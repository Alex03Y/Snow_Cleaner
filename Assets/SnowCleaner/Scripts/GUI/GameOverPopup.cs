using SnowCleaner.Scripts.Core.WindowManager;
using SnowCleaner.Scripts.Enums;

namespace SnowCleaner.Scripts.GUI
{
    public class GameOverPopup : IWindowController
    {
        public WindowView View { get; set; }
        public int WindowId => (int) WindowType.GameOver;
        public readonly GameStatus _status;

        public GameOverPopup(GameStatus status)
        {
            _status = status;
        }
    }
}