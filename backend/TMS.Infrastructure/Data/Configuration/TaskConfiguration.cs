using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.ValueObjects;
using DomainTaskStatus = TMS.Domain.ValueObjects.TaskStatus;

namespace TMS.Infrastructure.Data.Configuration
{
    public sealed class TaskConfiguration : IEntityTypeConfiguration<TMS.Domain.Entities.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasMaxLength(2000);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion(
                    status => status.Value,
                    value => DomainTaskStatus.FromString(value)
                )
                .HasMaxLength(50);

            builder.Property(t => t.Priority)
                .IsRequired()
                .HasConversion(
                    priority => priority.Value,
                    value => TaskPriority.FromString(value)
                )
                .HasMaxLength(50);

            builder.Property(t => t.DueDate);

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.UpdatedAt);

            builder.Property(t => t.ProjectId)
                .IsRequired();

            builder.Property(t => t.AssignedToUserId)
                .IsRequired();

            builder.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.AssignedToUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => t.Priority);
            builder.HasIndex(t => t.DueDate);
        }        
    }
}
