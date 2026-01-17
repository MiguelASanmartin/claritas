using FluentAssertions;
using Moq;
using TMS.Application.Commands;
using TMS.Application.Handlers;
using TMS.Domain.Entities;
using TMS.Domain.Interfaces;

namespace TMS.Tests.Handlers
{
    public class CreateTaskHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepo;
        private readonly Mock<IProjectRepository> _mockProjectRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;

        public CreateTaskHandlerTests() 
        {
            _mockTaskRepo = new Mock<ITaskRepository>();
            _mockProjectRepo = new Mock<IProjectRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
        }


        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_ReturnTaskResponse()
        {
            var command = new CreateTaskCommand(
                Title: "Test Task",
                Description: "Test Description",
                ProjectId: Guid.NewGuid(),
                AssignedToUserId: Guid.NewGuid(),
                Priority: "Medium",
                DueDate: DateTime.UtcNow.AddDays(7)
            );

            _mockProjectRepo
                .Setup(x => x.ExistsAsync(command.ProjectId))
                .ReturnsAsync(true);

            _mockUserRepo
                .Setup(x => x.ExistsAsync(command.AssignedToUserId))
                .ReturnsAsync(true);

            Domain.Entities.Task? savedTask = null;

            _mockTaskRepo
                .Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Task>()))
                .Callback<Domain.Entities.Task>(task => savedTask = task)
                .Returns(System.Threading.Tasks.Task.CompletedTask);            

            var mockTask = Domain.Entities.Task.Create(
                command.Title,
                command.ProjectId,
                command.AssignedToUserId
            );

            var mockProject = Project.Create("Test Project", command.AssignedToUserId);
            var mockUser = User.Create("Test User", "test@example.com");

            _mockTaskRepo
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() =>
                {
                    if (savedTask == null) return null;

                    typeof(Domain.Entities.Task).GetProperty("Project")!
                        .SetValue(mockTask, mockProject);
                    typeof(Domain.Entities.Task).GetProperty("AssignedToUser")!
                        .SetValue(mockTask, mockUser);

                    return savedTask;
                });

            var handler = new CreateTaskHandler(
                _mockTaskRepo.Object,
                _mockProjectRepo.Object,
                _mockUserRepo.Object                
            );

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be(command.Title);
            result.Description.Should().Be(command.Description);
            result.Priority.Should().Be(command.Priority);
            result.ProjectId.Should().Be(command.ProjectId);
            result.AssignedToUserId.Should().Be(command.AssignedToUserId);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ProjectDoesNotExist_ThrowsArgumentException()
        {
            var command = new CreateTaskCommand(
                Title: "Test Task",
                Description: "Description",
                ProjectId: Guid.NewGuid(),
                AssignedToUserId: Guid.NewGuid(),
                Priority: "High",
                DueDate: null
            );

            _mockProjectRepo
                .Setup(x => x.ExistsAsync(command.ProjectId))
                .ReturnsAsync(false);

            _mockUserRepo
                .Setup(x => x.ExistsAsync(command.AssignedToUserId))
                .ReturnsAsync(true);

            var handler = new CreateTaskHandler(
                _mockTaskRepo.Object,
                _mockProjectRepo.Object,
                _mockUserRepo.Object
            );

            var act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"*{command.ProjectId}*does not exist*");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_UserDoesNotExist_ThrowsArgumentException()
        {
            var command = new CreateTaskCommand(
                Title: "Test Task",
                Description: "Description",
                ProjectId: Guid.NewGuid(),
                AssignedToUserId: Guid.NewGuid(),
                Priority: "Low",
                DueDate: null
            );

            _mockProjectRepo
                .Setup(x => x.ExistsAsync(command.ProjectId))
                .ReturnsAsync(true);

            _mockUserRepo
                .Setup(x => x.ExistsAsync(command.AssignedToUserId))
                .ReturnsAsync(false);

            var handler = new CreateTaskHandler(
                _mockTaskRepo.Object,
                _mockProjectRepo.Object,
                _mockUserRepo.Object
            );

            var act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"*{command.AssignedToUserId}*does not exist*");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_CallsAddAsyncOnce()
        {
            var command = new CreateTaskCommand(
                Title: "Test Task",
                Description: null,
                ProjectId: Guid.NewGuid(),
                AssignedToUserId: Guid.NewGuid(),
                Priority: "Critical",
                DueDate: null
            );

            _mockProjectRepo
                .Setup(x => x.ExistsAsync(command.ProjectId))
                .ReturnsAsync(true);

            _mockUserRepo
                .Setup(x => x.ExistsAsync (command.AssignedToUserId))
                .ReturnsAsync(true);

            _mockTaskRepo
                .Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Task>()))
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            var mockTask = Domain.Entities.Task.Create(
                command.Title,
                command.ProjectId,
                command.AssignedToUserId
            );

            _mockTaskRepo
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockTask);

            var handler = new CreateTaskHandler(
                _mockTaskRepo.Object,
                _mockProjectRepo.Object,
                _mockUserRepo.Object
            );

            await handler.Handle(command, CancellationToken.None);

            _mockTaskRepo.Verify(
                x => x.AddAsync(It.IsAny<Domain.Entities.Task>()),
                Times.Once
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_GetByIdReturnsNull_ThrowsInvalidOperationException()
        {
            var command = new CreateTaskCommand(
                Title: "Test Task",
                Description: "Description",
                ProjectId: Guid.NewGuid(),
                AssignedToUserId: Guid.NewGuid(),
                Priority: "Medium",
                DueDate: null
            );

            _mockProjectRepo
                .Setup(x => x.ExistsAsync(command.ProjectId))
                .ReturnsAsync(true);

            _mockUserRepo
                .Setup(x => x.ExistsAsync(command.AssignedToUserId))
                .ReturnsAsync(true);

            _mockTaskRepo
                .Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Task>()))
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            _mockTaskRepo
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Domain.Entities.Task?) null);

            var handler = new CreateTaskHandler(
                _mockTaskRepo.Object,
                _mockProjectRepo.Object,
                _mockUserRepo.Object
            );

            var act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Failed to retrieve*");
        }
    }
}
