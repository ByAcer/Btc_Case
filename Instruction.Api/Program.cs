using FluentValidation.AspNetCore;
using Instruction.Api.Filters;
using Instruction.ApplicationService;
using Instruction.ApplicationService.MapProfiles;
using Instruction.Domain.Core;
using Instruction.Domain.MessageBroker;
using Instruction.Domain.Repositories;
using Instruction.Domain.Validations;
using Instruction.Repository.Core;
using Instruction.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<InstructionOrderCreateRequestDtoValidator>());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;

});

builder.Services.AddSingleton<MessageBrokerClientService>();
builder.Services.AddSingleton<MessageBrokerPublisher>();
builder.Services.AddSingleton((sp) => new ConnectionFactory()
{
    Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")),
    DispatchConsumersAsync = true

});
// Add services to the container.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IInstructionOrderRepository, InstructionOrderRepository>();
builder.Services.AddScoped<IInstructionApplicationService, InstructionApplicationService>();

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
);

builder.Services.AddAutoMapper(typeof(MapProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
