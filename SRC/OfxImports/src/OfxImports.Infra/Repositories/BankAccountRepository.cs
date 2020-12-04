using OfxImports.Domain.Entities;
using OfxImports.Domain.Interfaces;
using OfxImports.Infra.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tnf.EntityFrameworkCore;
using Tnf.EntityFrameworkCore.Repositories;

namespace OfxImports.Infra.Repositories
{
    public class BankAccountRepository : EfCoreRepositoryBase<OfxImportsContext, BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(IDbContextProvider<OfxImportsContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task AddBankAccount(BankAccount bankAccount)
        {
            Context.Add(bankAccount);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> BankAccountAlreadyExists(int code)
        {
            return await Context.BankAccounts.Any(x => x.Code == code).AsTask();
        }

        public async Task<Guid> GetIdByCode(int code)
        {
            return await Context.BankAccounts.FirstOrDefault(x => x.Code == code).Id.AsTask();
        }
    }
}
