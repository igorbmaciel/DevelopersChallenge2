using EFCore.BulkExtensions;
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
            var bulkConfig = new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true, BatchSize = 4000 };

            await Context.BulkInsertAsync(transactionList, bulkConfig);
            await Context.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await Context.Transactions.ToListAsync();
        }
    }
}
