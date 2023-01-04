using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Web.Models.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClienService _rabbitMQClienService;

        public RabbitMQPublisher(RabbitMQClienService rabbitMQClienService)
        {
            _rabbitMQClienService = rabbitMQClienService;
        }

        public void Publish(ProductImageCreatedEvent productImageCreatedEvent)
        {
            var channel = _rabbitMQClienService.Connect();
            var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);
            var bodyByte=Encoding.UTF8.GetBytes(bodyString);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange:RabbitMQClienService.ExchangeName,routingKey:RabbitMQClienService.RoutingWatermark, basicProperties: properties, body: bodyByte);
        }
    }
}
