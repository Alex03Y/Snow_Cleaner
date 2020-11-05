using SnowCleaner.Scripts.Core.ServiceLocator;

namespace SnowCleaner.Scripts.Core.Factory
{
    public interface IFactory : IService
    {
        void Initialize();
    }

    public interface IFactory<out TReturn, in TArgs> : IFactory
    {
        TReturn Create(TArgs args);
    }
}