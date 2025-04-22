using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Repositories;
using MediatR;
using System.Linq;
using AutoMapper;

namespace Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
        
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}