using System;
using System.Collections.Generic;
using System.Text;

namespace OfxImports.Domain.Queries.Response
{
    public class BankAccountResponse
    {
        public string Type { get; set; }

        public string AgencyCode { get; set; }

        public int Code { get; set; }

        public string AccountCode { get; set; }
    }
}
