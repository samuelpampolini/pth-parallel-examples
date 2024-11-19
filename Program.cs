// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Conference;

var services = new ServiceCollection();
services.AddLogging(builder =>
{
    builder.SetMinimumLevel(LogLevel.Trace);
    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
        options.TimestampFormat = "{HH:mm:ss} ";
        options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
    });
});

services.AddScoped<IExampleFactory, ExampleFactory>()
    .AddScoped<DeadLock>()
    .AddScoped<RaceCondition>()
    .AddScoped<ThreadContention>()
    .AddScoped<ThreadSafeQueue>();

var dependencyInjectionProvider = services.BuildServiceProvider();
var logger = dependencyInjectionProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Program");
var exampleFactory = dependencyInjectionProvider.GetRequiredService<IExampleFactory>();
CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
while (true)
{

    Console.Clear();
    logger.LogInformation("Press the number of the example you want to run:");
    logger.LogInformation("1 - DeadLock");
    logger.LogInformation("2 - RaceCondition");
    logger.LogInformation("3 - ThreadContention");
    logger.LogInformation("4 - ThreadSafeQueue");

    ConsoleKey key = Console.ReadKey().Key;
    if (key == ConsoleKey.Escape) break;

    var example = exampleFactory.CreateExample(key);
    await example.Run(cancellationTokenSource);

    logger.LogInformation("Press any key to clear the console and choose the next execution:");
    Console.ReadKey();
}

