using DomainTaskStatus = TMS.Domain.ValueObjects.TaskStatus;

namespace TMS.Domain.Entities
{
    public sealed class Project : BaseEntity
    {
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public DateTime? DueDate { get; private set; }

        // Foreign key
        public Guid OwnerId { get; private set; }

        public User Owner { get; private set; } = null!;
        public ICollection<Task> Tasks { get; private set; } = new List<Task>();

        private Project() { } //EF Core

        private Project(string name, Guid ownerId, string? description)
        {
            Name = name;
            OwnerId = ownerId;
            Description = description;
        }

        public static Project Create(string name, Guid ownerId, string? description = null)
        {
            if (string.IsNullOrEmpty(name)) 
                throw new ArgumentException("Project name cannot be empty", nameof(name));

            if (ownerId == Guid.Empty) 
                throw new ArgumentException("Owner ID cannot be empty", nameof(ownerId));

            return new Project(name, ownerId, description);
        }

        public void UpdateDescription(string? description)
        {
            Description = description;
            MarkAsUpdated();
        }

        public void SetDueDate(DateTime? dueDate)
        {
            DueDate = dueDate;
            MarkAsUpdated();
        }

        public bool IsOverdue() => DueDate.HasValue && DueDate.Value < DateTime.UtcNow;

        public int GetTaskCount() => Tasks.Count;
        public int GetCompletedTaskCount() => Tasks.Count(t => t.Status == DomainTaskStatus.Completed);
    }
}
