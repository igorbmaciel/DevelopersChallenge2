using FluentValidation.Results;
using System;

namespace OfxImports.Domain.Base
{
    public abstract class BaseCommand
    {
        public ValidationResult ValidationResult { get; set; }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
