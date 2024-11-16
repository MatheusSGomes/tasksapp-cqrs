using Application.UserCQ.Commands;
using Application.UserCQ.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infra.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração e injeção do serviço de banco de dados
builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Injeção do Command
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly));

// AddValidatorsFromAssemblyContaining <ClasseQueContémValidador>
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
// Usa os validadores no pipeline da requisição (validação do próprio ASP NET Core)
builder.Services.AddFluentValidationAutoValidation();

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
