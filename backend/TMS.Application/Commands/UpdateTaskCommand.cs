using MediatR;
using TMS.Application.DTOs.Responses;

namespace TMS.Application.Commands
{
    public sealed record UpdateTaskCommand(
        Guid Id,
        string? Title,
        string? Description,
        string? Priority,
        DateTime? DueDate
        ) : IRequest<TaskResponse>;
}
