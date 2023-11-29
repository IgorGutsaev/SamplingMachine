using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Identity.Abstractions;
using MPT.Vending.Domains.Identity.Infrastructure.Repositories;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public class IdentityService : IIdentityService
    {
        public IdentityService(UserRepository userRepository) {
            _userRepository = userRepository;
        }

        public IEnumerable<User> Get(Func<User, bool> predicate)
            => _userRepository.Get(x => predicate(new User { Email = x.Email, Password = x.Password, UID = x.Id, Admin = x.Admin })).ToList()
            .Select(x => new User { Email = x.Email, Password = x.Password, UID = x.Id, Admin = x.Admin });

        private readonly UserRepository _userRepository;
    }
}