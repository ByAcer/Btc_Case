using Instruction.Publisher.Domain.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Instruction.Publisher.MessageBroker
{
    public class Publisher
    {
        private readonly ClientService _rabbitMQClientService;

        public Publisher(ClientService rabbitMQClientService)
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
            channel.BasicPublish(ClientService.ExchangeName,ClientService.RoutingWatermark,properties,bodyByte);
        }
    }
}
