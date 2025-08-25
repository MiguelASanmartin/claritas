namespace TMS.Application.DTOs.Requests
{
    public class CreateTaskRequest
    {
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public Guid ProjectId { get; init; }
        public Guid AssignedToUserId { get; init; }
        public string Priority { get; init; } = "Medium";
        public DateTime? DueDate { get; init; }
    }
}
