using System;
using System.Collections.Generic;

namespace OfxImports.Domain.Entities
{
    public class BankAccount
    {

        public BankAccount()
        {

        }

        public Guid Id { get; set; }

        public string Type { get; private set; }

        public string AgencyCode { get; private set; }

        public int Code { get; private set; }

        public string AccountCode { get; private set; }

        public virtual List<Transaction> TransactionList { get; internal set; }

        public BankAccount(string type, string agencyCode, int code, string accountCode)
        {
            Id = Guid.NewGuid();
            Type = type;
            AgencyCode = agencyCode;
            Code = code;
            AccountCode = accountCode;
        }

        internal BankAccount AddBankAccount(string type, string agencyCode, int code, string accountCode)
        {
            return new BankAccount(type, agencyCode, code, accountCode);
        }     

    }
}
