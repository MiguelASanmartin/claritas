using TMS.Application.DTOs.Responses;
using TMS.Domain.Entities;

namespace TMS.Application.Extensions
{
    public static class MappingExtensions
    {
        public static TaskResponse ToResponse(this Domain.Entities.Task task)
        {
            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.Value,
                Priority = task.Priority.Value,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                UpdateAt = task.UpdatedAt,
                ProjectId = task.ProjectId,
                ProjectName = task.Project?.Name ?? string.Empty,
                AssignedToUserId = task.AssignedToUserId,
                AssignedToUserName = task.AssignedToUser?.Name ?? string.Empty
            };
        }

        public static ProjectResponse ToResponse(this Project project)
        {
            return new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                DueDate = project.DueDate,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                OwnerId = project.OwnerId,
                OwnerName = project.Owner?.Name ?? string.Empty,
                TaskCount = project.GetTaskCount(),
                CompletedTaskCount = project.GetCompletedTaskCount(),
                IsOverdue = project.IsOverdue()
            };
        }

        public static UserResponse ToResponse(this User user) 
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                ProjectCount = user.Projects.Count,
                AssignedTaskCount = user.AssignedTasks.Count
            };
        }

        public static IEnumerable<TaskResponse> ToResponse(this IEnumerable<Domain.Entities.Task> tasks)
        {
            return tasks.Select(t => t.ToResponse());
        }

        public static IEnumerable<ProjectResponse> ToResponse(this IEnumerable<Project> projects)
        {
            return projects.Select(p => p.ToResponse());
        }
    }
}
