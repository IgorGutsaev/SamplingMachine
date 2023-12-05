using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Identity.Abstractions;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public class DemoIdentityService : IIdentityService
    {
        public IEnumerable<User> Get(Func<User, bool> predicate) {
            return null;
        }

        public void Create(string email, string password) { }
    }
}