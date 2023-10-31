using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Products.Abstractions
{
    public interface ISessionService
    {
        event EventHandler<Session> OnNewSession;
        IAsyncEnumerable<Session> Get(SessionsRequest filter);
        void Put(Session session);
    }
}