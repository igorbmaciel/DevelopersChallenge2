using OfxImports.Domain.Entities;
using System.Collections.Generic;

namespace OfxImports.Domain.Queries.Response
{
    public class AddOfxImportResponse
    {
        public BankAccount BankAccount { get; set; }
        public List<Transaction> TransactionList { get; set; }
    }
}
