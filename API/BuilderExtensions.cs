using Application.UserCQ.Commands;
using Application.UserCQ.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API;

public static class BuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // Injeção do Command
        builder.Services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly));
    }

    public static void AddDatabase(this WebApplicationBuilder builder/*, IConfiguration configuration*/)
    {
        // Configuração e injeção do serviço de banco de dados
        var configuration = builder.Configuration; // Outra forma de usar configuration
        builder.Services.AddDbContext<TasksDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void AddValidations(this WebApplicationBuilder builder)
    {
        // AddValidatorsFromAssemblyContaining <ClasseQueContémValidador>
        builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
        // Usa os validadores no pipeline da requisição (validação do próprio ASP NET Core)
        builder.Services.AddFluentValidationAutoValidation();

    }
}
