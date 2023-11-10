using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Ordering.Abstractions;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Services
{
    public class DemoTransactionService : ITransactionService
    {
        public event EventHandler<Transaction> OnNewTransaction;

        public async IAsyncEnumerable<Transaction> Get(TransactionRequest filter) {
            foreach (var s in DemoData._transactions.Where(x =>
                (string.IsNullOrWhiteSpace(filter.MobileNumber) || x.PhoneNumber.Contains(filter.MobileNumber, StringComparison.InvariantCultureIgnoreCase))
                && x.Date >= filter.From && filter.To > x.Date))
                yield return s;
        }

        public void Put(Transaction transaction) {
            OnNewTransaction?.Invoke(this, transaction);

            // fill with product details
            foreach (var product in transaction.Items) {
                Product storedProduct = DemoData._products.First(x => x.Sku == product.Product.Sku);
                product.Product.Names = storedProduct.Names;
                product.Product.Picture = storedProduct.Picture;
            }

            DemoData._transactions.Add(transaction);
        }
    }
}