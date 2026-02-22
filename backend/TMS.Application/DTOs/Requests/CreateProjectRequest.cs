namespace TMS.Application.DTOs.Requests
{
    public sealed class CreateProjectRequest
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public Guid OwnerId { get; init; }
        public DateTime? DueDate { get; init; }
    }
}
