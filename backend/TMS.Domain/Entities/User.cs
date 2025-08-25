namespace TMS.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        public string Name { get; private set; } = null!;
        public string Email { get; private set; } = null!;

        public ICollection<Project> Projects { get; private set; } = new List<Project>();
        public ICollection<Task> AssignedTasks { get; private set; } = new List<Task>();

        private User() { } //EF Core

        private User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public static User Create(string name, string email)
        {
            if (string.IsNullOrEmpty(name)) 
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            if (!email.Contains("@"))
                throw new ArgumentException("Invalid email format", nameof(email));

            return new User(name, email);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            Name = name;
            MarkAsUpdated();
        }
    }
}
