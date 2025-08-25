using MediatR;
using TMS.Application.DTOs.Responses;

namespace TMS.Application.Queries
{
    public sealed record GetTasksByProjectQuery(Guid ProjectId) : IRequest<IEnumerable<TaskResponse>>;
}
