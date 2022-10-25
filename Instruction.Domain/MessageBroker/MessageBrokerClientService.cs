using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Instruction.Domain.MessageBroker;

public class MessageBrokerClientService
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    public static string ExchangeName = "InsructionNotificationDirectExchange";
    public static string RoutingWatermark = "insruction-notification-route";
    public static string QueueName = "insruction-notification-queue";

    private readonly ILogger<MessageBrokerClientService> _logger;

    public MessageBrokerClientService(ConnectionFactory connectionFactory, ILogger<MessageBrokerClientService> logger)
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
