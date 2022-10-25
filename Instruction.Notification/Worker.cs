using Instruction.Notification.Services;
using Polly.CircuitBreaker;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Instruction.Notification.Domain;
using Instruction.Notification.Domain.Events;
using Newtonsoft.Json;

namespace Instruction.Notification;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly RabbitMqClientService _rabbitMqClientService;
    private IModel _channel;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;

    private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _policy = Policy.HandleResult<HttpResponseMessage>(message => (int)message.StatusCode == 503).CircuitBreakerAsync(2, TimeSpan.FromSeconds(15));
    private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> asyncCircuitBreakerPolicyForSms = _policy;
    private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> asyncCircuitBreakerPolicyForMail = _policy;
    private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> asyncCircuitBreakerPolicyForNotification = _policy;

    public Worker(ILogger<Worker> logger, RabbitMqClientService rabbitMqClientService, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _rabbitMqClientService = rabbitMqClientService;
        _serviceScopeFactory = serviceScopeFactory;
        _httpClientFactory = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>();
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = _rabbitMqClientService.Connect();
        _channel.BasicQos(0, 1, false);
        return base.StartAsync(cancellationToken);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var consumer = new AsyncEventingBasicConsumer(_channel);
        _channel.BasicConsume(RabbitMqClientService.QueueName, false, consumer);
        consumer.Received += Consumer_Received;
        return Task.CompletedTask;
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            var obje = @event.Body.ToArray();
            var message =Encoding.UTF8.GetString(obje);
            var instructionOrderEvent = JsonConvert.DeserializeObject<InstructionOrderEvent>(message);
            HttpResponseMessage response;


            //TODO: Chain of Responsibility uygulanmalý.

            //for (int i = 0; i < instructionOrderEvent.NotificationType.Count(); i++)
            {
                var value = instructionOrderEvent.NotificationType;//[i];
                switch (value)
                {
                    case (int)Enums.NotificationType.Sms:
                        response = await SendMessageViaSms(Url: "https://sms.com", instructionOrderEvent);
                        StatusCheckAndBasicAck(@event, response);
                        break;
                    case (int)Enums.NotificationType.EMail:
                        response = await SendMessageViaMail(Url: "https://mail.com", instructionOrderEvent);
                        StatusCheckAndBasicAck(@event, response);
                        break;
                    case (int)Enums.NotificationType.Notification:
                        response = await SendMessageViaNotification(Url: "https://notification.com", instructionOrderEvent);
                        StatusCheckAndBasicAck(@event, response);
                        break;
                    default:
                        _logger.LogError($"{instructionOrderEvent.UserId} UserId'li {instructionOrderEvent.Id} Id'li Bilinmeyen gönderim tipi...");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        
    }

    private void StatusCheckAndBasicAck(BasicDeliverEventArgs @event, HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            _channel.BasicAck(@event.DeliveryTag, false);
        else
            throw new Exception(response.StatusCode.ToString());
    }

    private async Task<HttpResponseMessage> SendMessageViaSms(string Url, InstructionOrderEvent model)
    {
        if (asyncCircuitBreakerPolicyForSms.CircuitState == CircuitState.Open)
        {
            throw new Exception("Service is currently unavaible!");
        };
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        HttpResponseMessage response;
        using (var httpClient = _httpClientFactory.CreateClient())
        {
            //response = await asyncCircuitBreakerPolicyForSms.ExecuteAsync(() =>
            //httpClient.PostAsync(Url, content));
            response = new HttpResponseMessage(statusCode:System.Net.HttpStatusCode.OK);
        }
        _logger.LogInformation($"{model.UserId} UserId'li kullanýcý için ");
        return response;
    }
    private async Task<HttpResponseMessage> SendMessageViaMail(string Url, InstructionOrderEvent model)
    {
        if (asyncCircuitBreakerPolicyForMail.CircuitState == CircuitState.Open)
        {
            throw new Exception("Service is currently unavaible!");
        };
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        HttpResponseMessage response;
        using (var httpClient = _httpClientFactory.CreateClient())
        {
            //response = await asyncCircuitBreakerPolicyForMail.ExecuteAsync(() =>
            //httpClient.PostAsync(Url, content));
            response = new HttpResponseMessage(statusCode: System.Net.HttpStatusCode.OK);
        }
        _logger.LogInformation($"{model.UserId} UserId'li kullanýcý için ");
        return response;
    }
    private async Task<HttpResponseMessage> SendMessageViaNotification(string Url, InstructionOrderEvent model)
    {
        if (asyncCircuitBreakerPolicyForNotification.CircuitState == CircuitState.Open)
        {
            throw new Exception("Service is currently unavaible!");
        };
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        HttpResponseMessage response;
        using (var httpClient = _httpClientFactory.CreateClient())
        {
            //response = await asyncCircuitBreakerPolicyForNotification.ExecuteAsync(() =>
            //httpClient.PostAsync(Url, content));
            response = new HttpResponseMessage(statusCode: System.Net.HttpStatusCode.OK);
        }
        _logger.LogInformation($"{model.UserId} UserId'li kullanýcý için ");
        return response;
    }
}