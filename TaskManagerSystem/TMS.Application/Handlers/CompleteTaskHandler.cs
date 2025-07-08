using AutoMapper;
using MediatR;
using TMS.Application.Commands;
using TMS.Application.DTOs.Responses;
using TMS.Domain.Interfaces;

namespace TMS.Application.Handlers
{
    public sealed class CompleteTaskHandler : IRequestHandler<CompleteTaskCommand, TaskResponse>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly Mapper _mapper;

        public CompleteTaskHandler(ITaskRepository taskRepository, Mapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<TaskResponse> Handle(CompleteTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(command.Id);
            if (task == null) 
                throw new ArgumentException($"Task with ID {command.Id} not found.");

            task.Complete();

            await _taskRepository.UpdateAsync(task);

            return _mapper.Map<TaskResponse>(task);
        }
    }
}
