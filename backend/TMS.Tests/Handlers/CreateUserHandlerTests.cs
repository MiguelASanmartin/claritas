using FluentAssertions;
using Moq;
using System.Collections.Concurrent;
using TMS.Application.Commands;
using TMS.Application.Handlers;
using TMS.Domain.Entities;
using TMS.Domain.Interfaces;

namespace TMS.Tests.Handlers
{
    public class CreateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;

        public CreateUserHandlerTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_ReturnsUserResponse()
        {
            var command = new CreateUserCommand(
                Name: "Miguel",
                Email: "miguel@example.com"
            );

            User? savedUser = null;

            _mockUserRepo
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Callback<User>(user => savedUser = user)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            var handler = new CreateUserHandler(_mockUserRepo.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be(command.Name);
            result.Email.Should().Be(command.Email);
            result.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_CallsAddAsyncOnce()
        {
            var command = new CreateUserCommand(
                Name: "Test User",
                Email: "test@example.com"
            );

            _mockUserRepo
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            var handler = new CreateUserHandler( _mockUserRepo.Object);

            await handler.Handle(command, CancellationToken.None);

            _mockUserRepo.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Once
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_CreatesUserWithCorrectProperties()
        {
            var command = new CreateUserCommand(
                Name: "John Smith",
                Email: "john.smith@company.com"
            );

            User? capturedUser = null;

            _mockUserRepo
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Callback<User>(user => capturedUser = user)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            var handler = new CreateUserHandler(_mockUserRepo.Object);

            await handler.Handle(command, CancellationToken.None);

            capturedUser.Should().NotBeNull();
            capturedUser!.Name.Should().Be(command.Name);
            capturedUser.Email.Should().Be(command.Email);
            capturedUser.Id.Should().NotBeEmpty();
            capturedUser.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
