namespace TMS.Domain.ValueObjects
{
    public sealed class TaskPriority : IEquatable<TaskPriority>
    {
        public static readonly TaskPriority Low = new("Low");
        public static readonly TaskPriority Medium = new("Medium");
        public static readonly TaskPriority High = new("High");
        public static readonly TaskPriority Critical = new("Critical");

        public string Value { get; }

        public TaskPriority(string value) 
        { 
            Value = value; 
        }

        public static TaskPriority FromString(string priority)
        {
            return priority switch
            {
                "Low" => Low,
                "Medium" => Medium,
                "High" => High,
                "Critical" => Critical,
                _ => throw new ArgumentException($"Invalid task priority: {priority}")
            };
        }
        public bool Equals(TaskPriority? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj as TaskPriority);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(TaskPriority? left, TaskPriority? right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(TaskPriority? left, TaskPriority? right)
        {
            return !(left == right);
        }
    }
}
