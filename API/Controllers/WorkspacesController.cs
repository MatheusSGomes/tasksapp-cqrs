namespace API.Controllers;

/*
 * Controllers tradicionais vem do AspNet core - São mais pesados, tem muitas dependências.
 * Minimal Api's - São mais leves do que as controllers geradas pelo MVC.
 *
 * WebApplication != WebApplicationBuilder
 * WebApplicationBuilder - não fornece métodos de roteamento, fornece Services...
 */
public static class WorkspacesController
{
    public static void WorkspacesRoutes(this WebApplication app)
    {
        var group = app.MapGroup("Workspaces") // Group (é a rota)
            .WithTags("Workspaces"); // Tags (agrupamento de rotas no Swagger)

        group.MapGet("OlaMundo", () => "Olá mundo!");
    }
}
