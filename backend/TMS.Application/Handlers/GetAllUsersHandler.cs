using MediatR;
using TMS.Application.DTOs.Responses;
using TMS.Application.Extensions;
using TMS.Application.Queries;
using TMS.Domain.Interfaces;

namespace TMS.Application.Handlers
{
    public sealed class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();

            return users.ToResponse();
        }
    }
}
