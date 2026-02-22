using DomainTaskStatus =  TMS.Domain.ValueObjects.TaskStatus;
using TMS.Domain.ValueObjects;

namespace TMS.Domain.Entities
{
    public sealed class Task : BaseEntity
    {
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public DomainTaskStatus Status { get; private set; } = null!;
        public TaskPriority Priority { get; private set; } = null!;
        public DateTime? DueDate { get; private set; }

        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; } = null!;

        public Guid AssignedToUserId { get; private set; }
        public User AssignedToUser { get; private set; } = null!;

        private Task() { } //EF Core

        private Task(string title, Guid projectId, Guid assignedToUserId)
        {
            Title = title;
            ProjectId = projectId;
            AssignedToUserId = assignedToUserId;
            Status = DomainTaskStatus.Pending;
            Priority = TaskPriority.Medium;

        }

        public static Task Create(string title, Guid projectId, Guid assignedToUserId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title cannot be empty", nameof(title));

            if (projectId == Guid.Empty)
                throw new ArgumentException("Project ID cannot be empty", nameof(projectId));

            if (assignedToUserId == Guid.Empty)
                throw new ArgumentException("Assigned user ID cannot be empty", nameof(assignedToUserId));

            var task = new Task(title, projectId, assignedToUserId);

            // Domain event
            //task.AddDomainEvent(new TaskCreatedEvent(task.Id, task.Title, task.ProjectId));

            return task;
        }

        public void Complete()
        {
            if (Status == DomainTaskStatus.Completed)
                throw new InvalidOperationException("Task is already completed");

            Status = DomainTaskStatus.Completed;
            MarkAsUpdated();

            // Domain event
            //AddDomainEvent(new TaskCompletedEvent(Id, Title, ProjectId));
        }

        public void Cancel()
        {
            if (Status == DomainTaskStatus.Completed)
                throw new InvalidOperationException("Cannot cancel completed task");

            if (Status == DomainTaskStatus.Cancelled)
                throw new InvalidOperationException("Task is already cancelled");

            Status = DomainTaskStatus.Cancelled;
            MarkAsUpdated();
        }

        public void UpdateDescription(string description)
        {
            Description = description;
            MarkAsUpdated();
        }

        public void ChangePriority(TaskPriority priority)
        {
            Priority = priority ?? throw new ArgumentNullException(nameof(priority));
            MarkAsUpdated();
        }

        public void SetDueDate(DateTime? dueDate)
        {
            DueDate = dueDate;
            MarkAsUpdated();
        }

        public bool IsOverDue() => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != DomainTaskStatus.Completed;
        public bool IsHighPriority() => Priority == TaskPriority.High || Priority == TaskPriority.Critical;
    }
}
