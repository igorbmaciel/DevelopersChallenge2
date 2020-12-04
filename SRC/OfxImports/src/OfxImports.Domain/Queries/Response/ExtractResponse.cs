using System.Collections.Generic;

namespace OfxImports.Domain.Queries.Response
{
    public class ExtractResponse
    {
        public BankAccountResponse BankAccount { get; set; }

        public List<TransactionResponse> Transactions { get; set; }


        public ExtractResponse(BankAccountResponse bankAccount,
            string status)
        {
            Init(bankAccount, status);
        }

        private void Init(BankAccountResponse bankAccount, string status)
        {
            BankAccount = bankAccount;
            Transactions = new List<TransactionResponse>();
        }

        public void AddTransaction(TransactionResponse transaction)
        {
            if (Transactions == null)            
                Transactions = new List<TransactionResponse>();
            
            Transactions.Add(transaction);
        }
    }
}
