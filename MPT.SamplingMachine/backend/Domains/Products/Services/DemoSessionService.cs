using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Products.Abstractions;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Services
{
    public class DemoSessionService : ISessionService
    {
        public event EventHandler<Session> OnNewSession;

        public async IAsyncEnumerable<Session> Get(SessionsRequest filter) {
            foreach (var s in DemoData._sessions.Where(x =>
                (string.IsNullOrWhiteSpace(filter.MobileNumber) || x.PhoneNumber.Contains(filter.MobileNumber, StringComparison.InvariantCultureIgnoreCase))
                && x.Date >= filter.From && filter.To > x.Date))
                yield return s;
        }

        public void Put(Session session) {
            OnNewSession?.Invoke(this, session);

            // fill with product details
            foreach (var product in session.Items) {
                Product storedProduct = DemoData._products.First(x => x.Sku == product.Product.Sku);
                product.Product.Names = storedProduct.Names;
                product.Product.Picture = storedProduct.Picture;
            }

            DemoData._sessions.Add(session);
        }
    }
}