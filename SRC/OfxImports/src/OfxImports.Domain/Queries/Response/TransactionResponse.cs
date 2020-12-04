using System;

namespace OfxImports.Domain.Queries.Response
{
    public class TransactionResponse
    {
        public string Type { get; set; }

        public DateTime Date { get; set; }

        public double TransactionValue { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        public long Checksum { get; set; }
    }
}
