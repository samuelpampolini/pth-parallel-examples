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

//services.AddScoped<IExample, DeadLock>();
//services.AddScoped<IExample, RaceCondition>();
//services.AddScoped<IExample, ThreadSafeQueue>();

var dependencyInjectionProvider = services.BuildServiceProvider();
var logger = dependencyInjectionProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Program");

var example = dependencyInjectionProvider.GetRequiredService<IExample>();

do
{
    await example.Run();

    logger.LogInformation("Press ESC to exit or any other key to run again");
} while(Console.ReadKey().Key != ConsoleKey.Escape);
