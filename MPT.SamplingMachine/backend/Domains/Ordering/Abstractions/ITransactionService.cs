using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Ordering.Abstractions
{
    public interface ITransactionService
    {
        event EventHandler<Transaction> OnNewTransaction;
        IAsyncEnumerable<Transaction> Get(TransactionRequest filter);
        void Put(Transaction transaction);
    }
}