using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Instruction.Domain.MessageBroker;

public class MessageBrokerPublisher
{
    private readonly MessageBrokerClientService _rabbitMQClientService;

    public MessageBrokerPublisher(MessageBrokerClientService rabbitMQClientService)
    {
        _rabbitMQClientService = rabbitMQClientService;
    }

    public void Publish(object model)
    {
        var channel = _rabbitMQClientService.Connect();

        var bodyString = JsonSerializer.Serialize(model);

        var bodyByte = Encoding.UTF8.GetBytes(bodyString);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: MessageBrokerClientService.ExchangeName, 
            routingKey: MessageBrokerClientService.RoutingWatermark, 
            basicProperties: properties, 
            body: bodyByte);
    }
}
