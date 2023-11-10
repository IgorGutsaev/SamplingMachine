using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Ordering.Abstractions;
using MPT.Vending.Domains.Ordering.Infrastructure.Entities;
using MPT.Vending.Domains.Ordering.Infrastructure.Repositories;

namespace MPT.Vending.Domains.Ordering.Services
{
    public class TransactionService : ITransactionService
    {
        public event EventHandler<Transaction> OnNewTransaction;

        public TransactionService(TransactionRepository transactionRepository,
            CustomerRepository customerRepository,
            ProductRepository productRepository)
        {
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async IAsyncEnumerable<Transaction> Get(TransactionRequest filter) {
            IEnumerable<TransactionEntity> entities = _transactionRepository.Get(x => (string.IsNullOrWhiteSpace(filter.MobileNumber) || x.Customer.PhoneNumber.Contains(filter.MobileNumber, StringComparison.InvariantCultureIgnoreCase))
                && x.Date >= filter.From && filter.To > x.Date);

            foreach (TransactionEntity entity in entities) {
                yield return new Transaction {
                    PhoneNumber = entity.Customer.PhoneNumber,
                    Date = entity.Date,
                    Items = entity.Items.Select(x => new TransactionProductLink {
                        Count = x.Count,
                        UnitCredit = x.UnitCredit,
                        Product = new Product { Sku = x.Product.Sku }
                    })
                };
            }
        }

        public void Put(Transaction transaction) {
            OnNewTransaction?.Invoke(this, transaction);

            CustomerEntity customer = _customerRepository.Put(new CustomerEntity { PhoneNumber = transaction.PhoneNumber });

            var t = new TransactionEntity {
                CustomerId = customer.Id,
                Date = transaction.Date,
                Items = transaction.Items.Select(x => new TransactionItemEntity {
                    ProductId = _productRepository.Get(p => p.Sku == x.Product.Sku).First().Id,
                    Count = x.Count,
                    UnitCredit = x.UnitCredit
                }).ToList()
            };

            _transactionRepository.Put(t);
        }

        private readonly TransactionRepository _transactionRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
    }
}