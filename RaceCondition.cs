using Microsoft.Extensions.Logging;

namespace Conference;
internal sealed class RaceCondition : IExample
{
    private ILogger<RaceCondition> _logger;

    public int result = 0;

    public RaceCondition(ILogger<RaceCondition> logger)
    {
        _logger = logger;
    }

    void Work1() { result = 1; }
    void Work2() { result = 2; }
    void Work3() { result = 3; }

    public Task Run()
    {
        CancellationToken cts = new CancellationToken();

        _logger.LogInformation("Starting Possible Race Condition");

        var job = new RaceCondition(_logger);
        Thread worker1 = new Thread(job.Work1);
        Thread worker2 = new Thread(job.Work2);
        Thread worker3 = new Thread(job.Work3);
        worker1.Start();
        worker2.Start();
        worker3.Start();
        _logger.LogWarning("Result: {result}", job.result);

        return Task.CompletedTask;
    }
}
