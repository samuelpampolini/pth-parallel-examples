using Microsoft.Extensions.Logging;

namespace Conference;

internal sealed class ThreadContention : IExample
{
    private ILogger<ThreadContention> _logger;

    public int result = 0;

    private object lock1 = new object();

    public ThreadContention(ILogger<ThreadContention> logger)
    {
        _logger = logger;
    }

    public async Task Run(CancellationTokenSource cancellationTokenSource)
    {
        CancellationToken cts = cancellationTokenSource.Token;
        _logger.LogInformation("Starting Dead Lock Example");

        var job = new ThreadContention(_logger);
        await Parallel.ForAsync(0, 4, cts, async (index, ct) => await Work(index, ct));
    }

    private Task Work(int index, CancellationToken cancellationToken) {
        var random = new Random();
        using (_logger.BeginScope("[Thread {index}]", index))
        {
            for(int i = 0; i < 3; i++) {
                _logger.LogInformation("[{time:HH:mm:ss.fff}] Waiting to unlock lock...", DateTime.Now);
                lock(lock1) {
                     _logger.LogInformation("[{time:HH:mm:ss.fff}] Working...", DateTime.Now);
                    
                    Task.Delay(random.Next(100, 400), cancellationToken).Wait();

                    _logger.LogInformation("[{time:HH:mm:ss.fff}] Release Lock...", DateTime.Now);
                }
            }
        }

        return Task.CompletedTask;
    }
}
