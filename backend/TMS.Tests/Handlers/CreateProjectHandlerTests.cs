using FluentAssertions;
using Moq;
using System.Runtime.InteropServices;
using TMS.Application.Commands;
using TMS.Application.Handlers;
using TMS.Domain.Entities;
using TMS.Domain.Interfaces;

namespace TMS.Tests.Handlers
{
    public class CreateProjectHandlerTests
    {
        private readonly Mock<IProjectRepository> _mockProjectRepo;

        public CreateProjectHandlerTests()
        {
            _mockProjectRepo = new Mock<IProjectRepository>();
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_ReturnsProjectResponse()
        {
            var ownerId = Guid.NewGuid();
            var command = new CreateProjectCommand(
                Name: "Portfolio Project",
                OwnerId: ownerId,
                Description: "Task Management System",
                DueDate: null
            );

            Project? savedProject = null;

            _mockProjectRepo
                .Setup(x => x.AddAsync(It.IsAny<Project>()))
                .Callback<Project>(project => savedProject = project)
                .Returns(System.Threading.Tasks.Task.CompletedTask);
                
            _mockProjectRepo
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => savedProject);

            var handler = new CreateProjectHandler(_mockProjectRepo.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be(command.Name);
            result.Description.Should().Be(command.Description);
            result.OwnerId.Should().Be(command.OwnerId);
            result.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommandWithDueDate_SetsCorrectDueDate()
        {
            var dueDate = DateTime.UtcNow.AddMonths(3);
            var command = new CreateProjectCommand(
                Name: "Project with deadline",
                OwnerId: Guid.NewGuid(),
                Description: "Has due date",
                DueDate: dueDate
            );

            Project? savedProject = null;

            _mockProjectRepo
                .Setup(x => x.AddAsync(It.IsAny<Project>()))
                .Callback<Project>(project => savedProject = project)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            _mockProjectRepo
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() =>  savedProject);

            var handler = new CreateProjectHandler(_mockProjectRepo.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.DueDate.Should().BeCloseTo(dueDate, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommandWithoutDueDate_DueDateIsNull()
        {
            var command = new CreateProjectCommand(
                Name: "Project without deadline",
                OwnerId: Guid.NewGuid(),
                Description: "No due date",
                DueDate: null
            );

            Project? savedProject = null;

            _mockProjectRepo
                .Setup(x => x.AddAsync(It.IsAny<Project>()))
                .Callback<Project>(project => savedProject = project)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            _mockProjectRepo
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => savedProject);

            var handler = new CreateProjectHandler(_mockProjectRepo.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.DueDate.Should().BeNull();
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_CallsAddAsyncOnce()
        {
            var command = new CreateProjectCommand(
                Name: "Test Project",
                OwnerId: Guid.NewGuid(),
                Description: null,
                DueDate: null
            );

            Project? savedProject = null;

            _mockProjectRepo
                .Setup(x => x.AddAsync(It.IsAny<Project>()))
                .Callback<Project>(project => savedProject = project)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            _mockProjectRepo
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => savedProject);

            var handler = new CreateProjectHandler(_mockProjectRepo.Object);

            await handler.Handle(command, CancellationToken.None);

            _mockProjectRepo.Verify(
                x => x.AddAsync(It.IsAny<Project>()),
                Times.Once
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_GetByIdReturnsNull_ThrowsInvalidOperationException()
        {
            var command = new CreateProjectCommand(
                Name: "Test project",
                OwnerId: Guid.NewGuid(),
                Description: "Description",
                DueDate: null
            );

            _mockProjectRepo
                .Setup(x => x.AddAsync(It.IsAny<Project>()))
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            _mockProjectRepo
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Project?)null);

            var handler = new CreateProjectHandler(_mockProjectRepo.Object);

            var act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Failed to retrieve*");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_CreatesProjectWithCorrectProperties()
        {
            var ownerId = Guid.NewGuid();
            var command = new CreateProjectCommand(
                Name: "Enterprise project",
                OwnerId: ownerId,
                Description: "Clean architecture demo",
                DueDate: DateTime.UtcNow.AddDays(30)
            );

            Project? capturedProject = null;

            _mockProjectRepo
                .Setup(x => x.AddAsync(It.IsAny<Project>()))
                .Callback<Project>(project => capturedProject = project)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            _mockProjectRepo
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => capturedProject);

            var handler = new CreateProjectHandler(_mockProjectRepo.Object);

            await handler.Handle(command, CancellationToken.None);

            capturedProject.Should().NotBeNull();
            capturedProject!.Name.Should().Be(command.Name);
            capturedProject.Description.Should().Be(command.Description);
            capturedProject.OwnerId.Should().Be(command.OwnerId);
            capturedProject.DueDate.Should().BeCloseTo(command.DueDate!.Value, TimeSpan.FromSeconds(1));
            capturedProject.Id.Should().NotBeEmpty();
            capturedProject.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
