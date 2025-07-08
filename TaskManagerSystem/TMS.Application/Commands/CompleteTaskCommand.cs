using MediatR;
using TMS.Application.DTOs.Responses;

namespace TMS.Application.Commands
{
    public sealed record CompleteTaskCommand(Guid Id) : IRequest<TaskResponse>;
}
