using AutoMapper;
using MediatR;
using TMS.Application.DTOs.Responses;
using TMS.Application.Queries;
using TMS.Domain.Interfaces;

namespace TMS.Application.Handlers
{
    public sealed class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TaskResponse?>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetTaskByIdHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<TaskResponse?> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(query.Id);

            return task == null ? null : _mapper.Map<TaskResponse>(task);
        }
    }
}
