using MediatR;
using TMS.Application.DTOs.Responses;

namespace TMS.Application.Commands
{
    public sealed record CreateProjectCommand(
        string Name,
        string? Descripton,
        Guid OwnerId,
        DateTime? DueDate
        ) : IRequest<ProjectResponse>;
}
