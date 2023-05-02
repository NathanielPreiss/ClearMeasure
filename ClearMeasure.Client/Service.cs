namespace ClearMeasure.Client;

internal class Service : IHostedService
{
    private readonly IValueMapping _mapping;
    private readonly ILogger<Service> _logger;
    
    private const int UpperLimit = 100;

    public Service(
        IValueMapping mapping,
        ILogger<Service> logger)
    {
        _mapping = mapping;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service Starting");
        
        var mappingValues = _mapping.MapToUpperLimit(UpperLimit).ToList();

        mappingValues.ForEach(x => Console.WriteLine(string.Join(" ", x)));

        return Task.CompletedTask;
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service Stopping");

        return Task.CompletedTask;
    }
}
