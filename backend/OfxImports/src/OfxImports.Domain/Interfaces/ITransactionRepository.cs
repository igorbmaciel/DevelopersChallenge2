using OfxImports.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OfxImports.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddTransactions(List<Transaction> transactionList);
        Task<List<Transaction>> GetAllTransactions();
    }
}
