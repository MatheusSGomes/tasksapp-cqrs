using Application.Response;
using Application.WorkspaceCQ.Commands;
using Application.WorkspaceCQ.Queries;
using Application.WorkspaceCQ.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        /* Dica: Injeção do Mediator, sempre assíncrono */
        /*group.MapPost("OlaMundo", async ([FromServices] IMediator _mediator, [FromBody] CreateUserCommand command) =>
            await _mediator.Send(command));*/

        // group.MapPost("teste", () => "Meu teste");

        /*
         * Segundo parâmetro é expressão Lambda -> Tipo Delegates -> Delega uma tarefa ao sistema.
         * Forma errada: group.MapPost("create-workspace", () => CreateWorkspace);
         */

        group.MapPost("create-workspace", CreateWorkspace) /*.RequireAuthorization()*/;
        group.MapPut("edit-workspace", EditWorkspace);
        group.MapDelete("delete-workspace/{workspaceId}", DeleteWorkspace);
        group.MapGet("get-workspace", GetWorkspace);
        group.MapGet("get-all-workspaces", GetAllWorkspaces);
    }

    public static async Task<IResult> CreateWorkspace(
        [FromServices] IMediator _mediator,
        [FromBody] CreateWorkspaceCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.ResponseInfo is null)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.ResponseInfo);
    }

    public static async Task<IResult> EditWorkspace(
        [FromServices] IMediator _mediator,
        [FromBody] EditWorkspaceCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.ResponseInfo is null)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.ResponseInfo);
    }

    public static async Task<IResult> DeleteWorkspace(
        [FromServices] IMediator _mediator,
        [FromQuery] Guid workspaceId)
    {
        var result = await _mediator.Send(new DeleteWorkspaceCommand
        {
            Id = workspaceId
        });

        if (result.ResponseInfo is null)
            return Results.NoContent();

        return Results.BadRequest(result.ResponseInfo);
    }

    public static async Task<IResult> GetWorkspace([FromServices] IMediator _mediator, Guid workspaceId)
    {
        ResponseBase<WorkspaceViewModel> result = await _mediator.Send(new GetWorkspaceQuery { Id = workspaceId });

        if (result.ResponseInfo is null)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.ResponseInfo);
    }

    public static async Task<IResult> GetAllWorkspaces(
        [FromServices] IMediator _mediator,
        [FromQuery] Guid userId,
        [FromQuery] int pageSize,
        [FromQuery] int pageIndex)
    {
        var result = await _mediator.Send(new GetAllWorkspacesQuery
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            UserId = userId
        });

        if (result.ResponseInfo is null)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.ResponseInfo);
    }
}
