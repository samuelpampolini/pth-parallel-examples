using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Conference;

public interface IExampleFactory
{
    IExample CreateExample(string type);
}

internal sealed class ExampleFactory(IServiceProvider serviceProvider, ILogger<ExampleFactory>  logger) : IExampleFactory
{
    public IExample CreateExample(string type)
    {
        logger.LogInformation("Creating example of type {type}", type);

        switch (type)
        {
            case "D1":
                return serviceProvider.GetService<DeadLock>();
            case "D2":
                return serviceProvider.GetService<RaceCondition>();
            case "D3":
                return serviceProvider.GetService<ThreadSafeQueue>();
            default:
                throw new ArgumentException("Invalid type", nameof(type));
        }
    }
}