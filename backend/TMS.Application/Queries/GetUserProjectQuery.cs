using MediatR;
using TMS.Application.DTOs.Responses;

namespace TMS.Application.Queries
{
    public sealed record GetUserProjectsQuery(Guid UserId) : IRequest<IEnumerable<ProjectResponse>>;
}
