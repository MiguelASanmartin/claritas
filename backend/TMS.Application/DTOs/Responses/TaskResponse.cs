namespace TMS.Application.DTOs.Responses
{
    public sealed class TaskResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Priority { get; init; } = string.Empty;
        public DateTime? DueDate { get; init; }
        public DateTime? CreatedAt { get; init; }
        public DateTime? UpdateAt { get; init; }

        public Guid ProjectId { get; init; }
        public string ProjectName { get; init; } = string.Empty;

        public Guid AssignedToUserId { get; init; }
        public string AssignedToUserName { get; init; } = string.Empty;
    }
}
