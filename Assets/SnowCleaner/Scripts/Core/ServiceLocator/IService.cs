using System;

namespace SnowCleaner.Scripts.Core.ServiceLocator
{
    public interface IService
    {
        Type ServiceType { get; }
    }
}