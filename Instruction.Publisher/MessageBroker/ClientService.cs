using RabbitMQ.Client;

namespace Instruction.Publisher.MessageBroker;

public class ClientService
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    public static string ExchangeName = "insruction-notification-direct-exchange";
    public static string RoutingWatermark = "insruction-notification-route";
    public static string QueueName = "insruction-notification-queue";

    private readonly ILogger<ClientService> _logger;

    public ClientService(ConnectionFactory connectionFactory, ILogger<ClientService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;

    }

    public IModel Connect()
    {
        _connection = _connectionFactory.CreateConnection();


        if (_channel is { IsOpen: true })
        {
            return _channel;
        }

        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(ExchangeName, type: "direct", true, false);

        _channel.QueueDeclare(QueueName, true, false, false, null);


        _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWatermark);

        _logger.LogInformation("RabbitMQ ile bağlantı kuruldu...");


        return _channel;

    }
}
