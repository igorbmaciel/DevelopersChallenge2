using System;

namespace OfxImports.Domain.Entities
{
    public class Transaction
    {
        public Transaction()
        {

        }

        public Guid Id { get; set; }

        public Guid BankAccountId { get; set; }

        public string Type { get; private set; }

        public DateTime Date { get; private set; }

        public double TransactionValue { get; private set; }

        public string TransactionId { get; private set; }

        public string Description { get; private set; }

        public long Checksum { get; private set; }

        public virtual BankAccount BankAccount { get; internal set; }

        public Transaction(string type, DateTime date, double transactionValue, string transactionId, string description, long checksum, Guid bankAccountId)
        {
            Id = Guid.NewGuid();
            BankAccountId = bankAccountId;
            Type = type;
            Date = date;
            TransactionValue = transactionValue;
            TransactionId = transactionId;
            Description = description;
            Checksum = checksum;
        }

        internal Transaction AddTransaction(string type, DateTime date, double transactionValue, string transactionId, string description, long checksum, Guid bankAccountId)
        {
            return new Transaction(type, date, transactionValue, transactionId, description, checksum, bankAccountId);
        }

    }
}
