using Marraia.Queue;

namespace PaymentWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IDisposable _disposable;
    private readonly Consumer _consumer;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, Consumer consumer, IConfiguration configuration)
    {
        _logger = logger;
        _consumer = consumer;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _disposable = _consumer.Start<OrderMessage>(_configuration.GetSection("RabbitMq:QueuePayment").Value, async (message) =>
        {
            _logger.LogInformation("Received OrderMessage: {OrderId}, Customer: {CustomerName}, Email: {CustomerEmail}, Total: {TotalAmount}",
                message.OrderId, message.CustomerName, message.CustomerEmail, message.TotalAmount);
        });
    }
}
