using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Identity.Abstractions
{
    public interface IIdentityService
    {
        IEnumerable<User> Get(Func<User, bool> predicate);
        void Create(string email, string password);
    }
}