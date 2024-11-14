using Microsoft.Extensions.Logging;

namespace Conference;
internal sealed class DeadLock : IExample
{
    private ILogger<DeadLock> _logger;

    public int result = 0;

    private object lock1 = new object();
    private object lock2 = new object();

    public DeadLock(ILogger<DeadLock> logger)
    {
        _logger = logger;
    }

    public Task Run(CancellationTokenSource cancellationTokenSource)
    {
        CancellationToken cts = cancellationTokenSource.Token;
        _logger.LogInformation("Starting Dead Lock Example");

        var job = new DeadLock(_logger);

        Task work1 = Task.Run(job.Work1, cts);
        Task work2 = Task.Run(job.Work2, cts);

        Task.WaitAll(work1, work2);

        _logger.LogWarning("Result: {result}", job.result);

        return Task.CompletedTask;
    }

    void Work1() {
        lock (lock1) {
            _logger.LogInformation("Thread 1: Holding lock1...");
            Thread.Sleep(1000);

            _logger.LogInformation("Thread 1: Waiting for lock2...");
            lock (lock2) {
                _logger.LogInformation("Thread 1: Acquired lock2!");
                result = 1;
            }
        }
    }
    void Work2() {
        lock (lock2) {
            _logger.LogInformation("Thread 2: Holding lock2...");
            Thread.Sleep(1000);
            
            _logger.LogInformation("Thread 2: Waiting for lock1...");
            lock (lock1) {
                _logger.LogInformation("Thread 2: Acquired lock1!");
                result = 2;
            }
        }
    }
}
