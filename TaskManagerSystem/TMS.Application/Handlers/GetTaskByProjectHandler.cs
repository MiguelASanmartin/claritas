using AutoMapper;
using MediatR;
using TMS.Application.DTOs.Responses;
using TMS.Application.Queries;
using TMS.Domain.Interfaces;

namespace TMS.Application.Handlers
{
    public sealed class GetTaskByProjectHandler : IRequestHandler<GetTasksByProjectQuery, IEnumerable<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetTaskByProjectHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskResponse>> Handle(GetTasksByProjectQuery query, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetByProjectIdAsync(query.ProjectId);

            return _mapper.Map<IEnumerable<TaskResponse>>(tasks);
        }
    }
}
