using Microsoft.Extensions.Logging.Abstractions;

namespace CoffeeHub.Tests.Common;

public abstract class ServiceTestBase
{
    protected static NullLogger<T> Logger<T>() where T : class
    {
        return NullLogger<T>.Instance;
    }
}
