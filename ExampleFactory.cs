using System.Collections.Immutable;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Conference;

public interface IExampleFactory
{
    IExample CreateExample(ConsoleKey type);
    void PrintCommands();
}

internal sealed class ExampleFactory : IExampleFactory
{
    private ImmutableSortedDictionary<ConsoleKey, IExample> _examples;
    private IServiceProvider _serviceProvider;
    private ILogger<ExampleFactory> _logger;

    public ExampleFactory(IServiceProvider serviceProvider, ILogger<ExampleFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _examples = ImmutableSortedDictionary<ConsoleKey, IExample>.Empty;

        LoadExamples();
    }

    private void LoadExamples()
    {
        var loadingDictionary = new Dictionary<ConsoleKey, IExample>();
        Assembly.GetExecutingAssembly()
           .GetTypes()
           .Where(t => t.GetCustomAttributes<ExampleAttribute>().Any())
           .ToList()
           .ForEach(t =>
           {
               var attribute = t.GetCustomAttribute<ExampleAttribute>();
               var example = _serviceProvider.GetService(t) as IExample;

               if (example != null && attribute != null)
                   loadingDictionary.Add(attribute.Key, example);
           });

        _examples = loadingDictionary.ToImmutableSortedDictionary();
    }

    public IExample CreateExample(ConsoleKey type)
    {
        _logger.LogInformation("Creating example of type {type}", type);

        if (_examples.ContainsKey(type))
        {
            return _examples[type];
        }

        throw new ArgumentException("Invalid type", nameof(type));
    }

    public void PrintCommands()
    {
        _examples.ToList().ForEach(e =>
        {
            _logger.LogInformation("{key} - {name}", e.Key, e.Value.GetType().Name);
        });
    }
}