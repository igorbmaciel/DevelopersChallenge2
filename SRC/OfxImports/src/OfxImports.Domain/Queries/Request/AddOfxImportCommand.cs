using FluentValidation;
using MediatR;
using OfxImports.Domain.Base;
using OfxImports.Domain.Queries.Response;

namespace OfxImports.Domain.Queries.Request
{
    public class AddOfxImportCommand : BaseCommand, IRequest<AddOfxImportResponse>
    {
        public string FileName { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AddOfxImportValidator().Validate(this);

            return ValidationResult.IsValid;
        }

        public class AddOfxImportValidator : AbstractValidator<AddOfxImportCommand>
        {
            public AddOfxImportValidator()
            {
                RuleFor(e => e.FileName)
                    .NotEmpty()
                    .WithState(e => EntityError.InvalidFileName);
               
            }

            public enum EntityError
            {
                InvalidFileName
            }
        }
    }
}
