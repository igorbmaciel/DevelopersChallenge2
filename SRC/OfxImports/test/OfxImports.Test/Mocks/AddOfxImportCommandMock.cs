using OfxImports.Domain.Queries.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfxImports.Test.Mocks
{
    public class AddOfxImportCommandMock
    {
        public static AddOfxImportCommand GetValidDto()
        {
            return new AddOfxImportCommand()
            {
                FileName = "extrato1.ofx"
            };
        }

        public static AddOfxImportCommand GetInvalidDto()
        {
            return new AddOfxImportCommand()
            {
                FileName = string.Empty
            };
        }

        public static AddOfxImportCommand GetInvalidTypeDto()
        {
            return new AddOfxImportCommand()
            {
                FileName = "teste.txt"
            };
        }

        public static AddOfxImportCommand GetInvalidFileDto()
        {
            return new AddOfxImportCommand()
            {
                FileName = "teste.ofx"
            };
        }
    }
}
