namespace TMS.Application.DTOs.Responses
{
    public sealed class UserResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }

        public int ProjectCount { get; init; }
        public int AssignedTaskCount { get; init; }
    }
}
