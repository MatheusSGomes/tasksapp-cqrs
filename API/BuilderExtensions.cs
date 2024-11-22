using System.Reflection;
using System.Text;
using Application.Mappings;
using Application.UserCQ.Commands;
using Application.UserCQ.Validators;
using Domain.Abstractions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infra.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Services.AuthService;

namespace API;

public static class BuilderExtensions
{
    public static void AddSwaggerDocs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Tasks App",
                Description = "Aplicativo de tarefas baseado no Trello escrito em ASP .NET Core v8",
                Contact = new OpenApiContact
                {
                    Name = "Exemplo de página de contato",
                    Url = new Uri("https://meusite.com/contato")
                },
                License = new OpenApiLicense
                {
                    Name = "Exemplo de página de licença",
                    Url = new Uri("https://meusite.com/license")
                }
            });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    public static void AddJwtAuth(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidAudience = configuration["JWT:Audience"],
                ValidIssuer = configuration["JWT:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
            })
            // Configuração do Cookies HTTP Read Only
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Exige que requisição seja HTTPS
                options.Cookie.SameSite = SameSiteMode.Strict; // Bloquea cookie de terceiros (só aceita a origem definida)
                options.ExpireTimeSpan = TimeSpan.FromDays(7); // Verifica se o token tem menos de 7 dias (se não está expirado)
            });
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
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

    public static void AddMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(ProfileMappings).Assembly);
    }

    public static void AddInjections(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();
    }
}
