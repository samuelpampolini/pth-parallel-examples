using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Conference;

public interface IExampleFactory
{
    IExample CreateExample(ConsoleKey type);
}

internal sealed class ExampleFactory(IServiceProvider serviceProvider, ILogger<ExampleFactory>  logger) : IExampleFactory
{
    public IExample CreateExample(ConsoleKey type)
    {
        logger.LogInformation("Creating example of type {type}", type);

        switch (type)
        {
            case ConsoleKey.D1:
                return serviceProvider.GetService<DeadLock>();
            case ConsoleKey.D2:
                return serviceProvider.GetService<RaceCondition>();
            case ConsoleKey.D3:
                return serviceProvider.GetService<ThreadContention>();
            case ConsoleKey.D4:
                return serviceProvider.GetService<ThreadSafeQueue>();
            default:
                throw new ArgumentException("Invalid type", nameof(type));
        }
    }
}