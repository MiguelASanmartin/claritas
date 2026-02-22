using MediatR;
using TMS.Application.DTOs.Responses;

namespace TMS.Application.Commands
{
    public sealed record CreateTaskCommand(
        string Title,
        string? Description,
        Guid ProjectId,
        Guid AssignedToUserId,
        string Priority,
        DateTime? DueDate
        ) : IRequest<TaskResponse>;
}
