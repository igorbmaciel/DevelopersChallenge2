using Microsoft.EntityFrameworkCore;
using OfxImports.Domain.Entities;
using OfxImports.Domain.Interfaces;
using OfxImports.Infra.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tnf.EntityFrameworkCore;
using Tnf.EntityFrameworkCore.Repositories;

namespace OfxImports.Infra.Repositories
{
    public class TransactionRepository : EfCoreRepositoryBase<OfxImportsContext, Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDbContextProvider<OfxImportsContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task AddTransactions(List<Transaction> transactionList)
        {
            Context.AddRange(transactionList);

            await Context.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await Context.Transactions.ToListAsync();
        }
    }
}
