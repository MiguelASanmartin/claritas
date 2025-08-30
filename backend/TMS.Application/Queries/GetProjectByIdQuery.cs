using MediatR;
using TMS.Application.DTOs.Responses;

namespace TMS.Application.Queries
{
    public sealed record GetProjectByIdQuery(Guid Id) : IRequest<ProjectResponse?>;
}
