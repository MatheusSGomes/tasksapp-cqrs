using Application.Response;
using Application.UserCQ.ViewModels;
using MediatR;

namespace Application.UserCQ.Commands;

public record RefreshTokenCommand(string? Username, string? RefreshToken)
    : IRequest<ResponseBase<RefreshTokenViewModel>>;
