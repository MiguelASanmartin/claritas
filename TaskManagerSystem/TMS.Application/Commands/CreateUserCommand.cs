using MediatR;
using TMS.Application.DTOs.Responses;

namespace TMS.Application.Commands
{
    public sealed record CreateUserCommand(
        string Name,
        string Email
        ) : IRequest<UserResponse>;
}
