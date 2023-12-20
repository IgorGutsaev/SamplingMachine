using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Identity.Abstractions;
using MPT.Vending.Domains.Identity.Infrastructure.Entities;
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
            .Select(x => new User { Email = x.Email, Password = x.Password, UID = x.Id, Admin = x.Admin, Claims = x.Claims?.Select(c => c.Claim.Name).Distinct() });

        public void Create(string email, string password) {
            email = email.Trim().ToLower();
            password = password.Trim();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email and password are mandatory");

            _userRepository.Put(new UserEntity { Email = email, Password = password });
        }

        private readonly UserRepository _userRepository;
    }
}