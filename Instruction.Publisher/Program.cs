using Instruction.Publisher;
using Instruction.Publisher.MessageBroker;
using RabbitMQ.Client;
using Microsoft.EntityFrameworkCore;
using Instruction.Publisher.Domain.Core;
using Instruction.Publisher.Repository;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        //services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase(ctx.Configuration.GetConnectionString("DbServer")));
        services.AddSingleton<ClientService>();
        services.AddSingleton<Publisher>();
        services.AddSingleton(sp => new ConnectionFactory()
        {
            Uri = new Uri(ctx.Configuration.GetConnectionString("RabbitMQ")),
            DispatchConsumersAsync = true
        });
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<AppDbContext>(o => o.UseSqlServer(ctx.Configuration.GetConnectionString("SqlServer")));


        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
