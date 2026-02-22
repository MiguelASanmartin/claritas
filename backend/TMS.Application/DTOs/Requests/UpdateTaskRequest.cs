namespace TMS.Application.DTOs.Requests
{
    public sealed class UpdateTaskRequest
    {
        public string? Title { get; init; }
        public string? Description { get; init; }
        public string? Priority { get; init; }
        public DateTime? DueDate { get; init; }
    }
}
