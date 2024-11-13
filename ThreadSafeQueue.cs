using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Conference;

internal sealed class ThreadSafeQueue: IExample
{
    private ILogger<ThreadSafeQueue> _logger;

    public ThreadSafeQueue(ILogger<ThreadSafeQueue> logger)
    {
        _logger = logger;
    }

    ConcurrentQueue<string> bufferFromFile = new ConcurrentQueue<string>();
    bool endOfFile = false;

    public async Task Run()
    {
        _logger.LogInformation("Starting the Porto Tech hub Job");

        CancellationToken cts = new CancellationToken();

        Task readFile = ReadFile(cts);
        Task process = Parallel.ForAsync(0, 3, cts, async (index, ct) => await ProcessLine(index, ct));

        await Task.WhenAll(readFile, process);
    }

    private async Task ReadFile(CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("Producer"))
        {
            _logger.LogInformation("Start reading the file");
            await Task.Delay(500, cancellationToken);

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                string? line;
                while ((line = await sr.ReadLineAsync(cancellationToken: cancellationToken)) != null)
                {
                    bufferFromFile.Enqueue(line);
                    await Task.Delay(50, cancellationToken);
                }
            }

            endOfFile = true;
            _logger.LogInformation("Finish reading the file");
        }
    }

    private async Task ProcessLine(int index, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("[Consumer {index}]", index))
        {
            while (!endOfFile || !bufferFromFile.IsEmpty)
            {
                if (bufferFromFile.TryDequeue(out string? line))
                {
                    _logger.LogWarning(line);
                    await Task.Delay(30, cancellationToken);
                }
                else
                {
                    _logger.LogTrace("Waiting the producer");
                    await Task.Delay(500, cancellationToken);
                }
            }
        }
    }
}
