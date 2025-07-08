namespace TMS.Application.DTOs.Responses
{
    public sealed class ProjectResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Descripton { get; init; }
        public DateTime? DueDate { get; init; }
        public DateTime? CreatedAt { get; init; }
        public DateTime? UpdateAt { get; init; }

        public Guid OwnerId { get; init; }
        public string OwnerName { get; init; } = string.Empty;

        public int TaskCount { get; init; }
        public int CompletedTaskCount { get; init; }
        public bool IsOverdue { get; init; }
    }
}
