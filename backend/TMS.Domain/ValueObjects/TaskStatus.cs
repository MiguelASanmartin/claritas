using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain.ValueObjects
{
    public sealed class TaskStatus : IEquatable<TaskStatus>
    {
        public static readonly TaskStatus Pending = new ("Pending");
        public static readonly TaskStatus InProgress = new ("InProgress");
        public static readonly TaskStatus Completed = new ("Completed");
        public static readonly TaskStatus Cancelled = new ("Cancelled");

        public string Value { get; }

        private TaskStatus(string value)
        {
            Value = value;
        }

        public static TaskStatus FromString(string status)
        {
            return status switch
            {
                "Pending" => Pending,
                "InProgress" => InProgress,
                "Competed" => Completed,
                "Cancelled" => Cancelled,
                _ => throw new ArgumentException($"Invalid task status: {status}")
            };
        }

        public bool Equals(TaskStatus? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj as TaskStatus);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(TaskStatus? left, TaskStatus? right) 
        {
            return left ?.Equals(right) ?? right is null;
        }

        public static bool operator !=(TaskStatus? left, TaskStatus? right)
        {
            return !(left == right);
        }

    }
}
