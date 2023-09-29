using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Products.Abstractions
{
    public interface ISessionService
    {
        event EventHandler<Session> OnNewSession;
        void Put(Session session);
        IEnumerable<Session> Get(SessionsRequest request);
    }
}