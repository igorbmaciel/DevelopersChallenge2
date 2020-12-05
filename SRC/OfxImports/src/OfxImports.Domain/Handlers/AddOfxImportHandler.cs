using MediatR;
using OfxImports.Domain.Base;
using OfxImports.Domain.Entities;
using OfxImports.Domain.Factory;
using OfxImports.Domain.Interfaces;
using OfxImports.Domain.Queries.Request;
using OfxImports.Domain.Queries.Response;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tnf.Notifications;
using Tnf.Repositories.Uow;

namespace OfxImports.Domain.Handlers
{
    public class AddOfxImportHandler : BaseRequestHandler, IRequestHandler<AddOfxImportCommand, AddOfxImportResponse>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public AddOfxImportHandler(
        INotificationHandler notification,
        IUnitOfWorkManager unitOfWorkManager,
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository
        ) : base(notification)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _bankAccountRepository = bankAccountRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<AddOfxImportResponse> Handle(AddOfxImportCommand command, CancellationToken cancellationToken)
        {
            if (!IsValid(command))
                return null;

            var directoryFile = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(directoryFile, @"..\..\..\..\OFX\"));

            var extractResponse = OfxImportFactory.GetExtract($"{newPath}{command.FileName}", new ParserSettingsResponse(), _notification);

            if (_notification.HasNotification())
                return null;

            var bankAccount = new BankAccount();
            var transaction = new Transaction();

            bankAccount = bankAccount.AddBankAccount(extractResponse.BankAccount.Type, extractResponse.BankAccount.AgencyCode, extractResponse.BankAccount.Code, extractResponse.BankAccount.AccountCode);

            var bankAccountAlreadyExists = await _bankAccountRepository.BankAccountAlreadyExists(bankAccount.Code);

            if (bankAccountAlreadyExists)
            {
                var bankAccountId = await _bankAccountRepository.GetIdByCode(bankAccount.Code);
                extractResponse.Transactions.ForEach(t => transaction.AddTransaction(t.Type, t.Date, t.TransactionValue, t.Description, bankAccountId));
            }
            else
                extractResponse.Transactions.ForEach(t => transaction.AddTransaction(t.Type, t.Date, t.TransactionValue, t.Description, bankAccount.Id));

            var transactionOldList = await _transactionRepository.GetAllTransactions();

            var transactionsToAdd = SetTransactionList(transaction.TransactionList.ToList(), transactionOldList);

            using (var uow = _unitOfWorkManager.Begin())
            {
                if (!bankAccountAlreadyExists)
                    await _bankAccountRepository.AddBankAccount(bankAccount);

                if (transactionsToAdd.Any())
                    await _transactionRepository.AddTransactions(transactionsToAdd);

                uow.Complete();
            }

            return AddOfxImportResponse(bankAccount, transaction.TransactionList.ToList());
        }

        private List<Transaction> SetTransactionList(List<Transaction> transactionList, List<Transaction> transactionOldList)
        {
            var transactionsToAdd = new List<Transaction>();

            transactionList.ForEach(transaction =>
            {
                if (!transactionOldList.Any(x => x.TransactionValue == transaction.TransactionValue && x.Date.Date == transaction.Date.Date && 
                                                x.Description == transaction.Description && x.Type == transaction.Type))
                    transactionsToAdd.Add(transaction);
            });

            return transactionsToAdd;
        }

        private AddOfxImportResponse AddOfxImportResponse(BankAccount bankAccount, List<Transaction> transactionList)
        {
            return new AddOfxImportResponse()
            {
                BankAccount = bankAccount
            };
        }
    }
}
