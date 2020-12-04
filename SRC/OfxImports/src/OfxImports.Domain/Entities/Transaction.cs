using System;
using System.Collections.Generic;

namespace OfxImports.Domain.Entities
{
    public class Transaction
    {
        public Transaction()
        {
            TransactionList = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }

        public Guid BankAccountId { get; set; }

        public string Type { get; private set; }

        public DateTime Date { get; private set; }

        public double TransactionValue { get; private set; }

        public string Description { get; private set; }

        public virtual BankAccount BankAccount { get; internal set; }

        internal virtual ICollection<Transaction> TransactionList { get; set; }

        public Transaction(string type, DateTime date, double transactionValue, string description, Guid bankAccountId)
        {
            Id = Guid.NewGuid();
            BankAccountId = bankAccountId;
            Type = type;
            Date = date;
            TransactionValue = transactionValue;
            Description = description;
        }

        internal void AddTransaction(string type, DateTime date, double transactionValue, string description, Guid bankAccountId)
        {
            var transation =  new Transaction(type, date, transactionValue, description, bankAccountId);
            TransactionList.Add(transation);
        }

    }
}
