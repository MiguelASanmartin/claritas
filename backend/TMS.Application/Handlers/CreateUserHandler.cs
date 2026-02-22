using MediatR;
using TMS.Application.Commands;
using TMS.Application.DTOs.Responses;
using TMS.Application.Extensions;
using TMS.Domain.Entities;
using TMS.Domain.Interfaces;

namespace TMS.Application.Handlers
{
    public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = User.Create(command.Name, command.Email);

            await _userRepository.AddAsync(user);

            return user.ToResponse();
        }
    }
}
