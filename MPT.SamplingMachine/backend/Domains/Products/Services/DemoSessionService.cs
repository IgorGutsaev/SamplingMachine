using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Products.Abstractions;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Services
{
    public class DemoSessionService : ISessionService
    {
        public event EventHandler<Session> OnNewSession;

        public IEnumerable<Session> Get(SessionsRequest request)
        {
            string? phone = request.MobileNumber?.Trim();
            if (phone != null && phone.Length < 3)
                phone = null;

            return DemoData._sessions.Where(x => (request.From == DateTime.MinValue || x.Date >= request.From) && (request.To == DateTime.MinValue || x.Date <= request.To) && (phone == null || x.PhoneNumber.Contains(phone))).OrderByDescending(x => x.Date);
        }

        public void Put(Session session)
        {
            OnNewSession?.Invoke(this, session);

            // fill with product details
            foreach (var product in session.Items)
            {
                Product storedProduct = DemoData._products.First(x => x.Sku == product.Product.Sku);
                product.Product.Names = storedProduct.Names;
                product.Product.Picture = storedProduct.Picture;
            }

            DemoData._sessions.Add(session);
        }
    }
}
