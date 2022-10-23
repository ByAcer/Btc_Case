using FluentValidation.AspNetCore;
using Instruction.Api.Filters;
using Instruction.ApplicationService;
using Instruction.ApplicationService.MapProfiles;
using Instruction.Domain.Core;
using Instruction.Domain.Repositories;
using Instruction.Domain.Validations;
using Instruction.Repository.Core;
using Instruction.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<InstructionOrderCreateRequestDtoValidator>());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;

});


// Add services to the container.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IInstructionOrderRepository, InstructionOrderRepository>();
builder.Services.AddScoped<IInstructionApplicationService, InstructionApplicationService>();

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("InstructionOrderDb"));

builder.Services.AddAutoMapper(typeof(MapProfile));


//builder.Services.AddControllers();
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
