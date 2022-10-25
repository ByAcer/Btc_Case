using Instruction.Consumer;
using Instruction.Consumer.Services;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddHttpClient();
        services.AddSingleton<RabbitMqClientService>();
        services.AddSingleton(sp => new ConnectionFactory()
        {
            Uri = new Uri(ctx.Configuration.GetConnectionString("RabbitMQ")),
            DispatchConsumersAsync = true
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
