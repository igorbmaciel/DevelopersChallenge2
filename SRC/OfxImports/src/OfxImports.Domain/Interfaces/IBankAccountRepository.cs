using OfxImports.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace OfxImports.Domain.Interfaces
{
    public interface IBankAccountRepository
    {
        Task AddBankAccount(BankAccount bankAccount);
        Task<bool> BankAccountAlreadyExists(int code);
        Task<Guid> GetIdByCode(int code);
    }
}
