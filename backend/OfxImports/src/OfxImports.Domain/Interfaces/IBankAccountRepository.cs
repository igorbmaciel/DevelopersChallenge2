using OfxImports.Domain.Entities;
using System.Threading.Tasks;

namespace OfxImports.Domain.Interfaces
{
    public interface IBankAccountRepository
    {
        Task AddBankAccount(BankAccount bankAccount);
    }
}
