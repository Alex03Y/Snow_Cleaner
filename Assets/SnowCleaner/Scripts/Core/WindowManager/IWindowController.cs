namespace SnowCleaner.Scripts.Core.WindowManager
{
    public interface IWindowController
    {
        WindowView View { get; set; }
        int WindowId { get; }
    }
}