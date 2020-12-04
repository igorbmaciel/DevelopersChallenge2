using FluentValidation;
using MediatR;
using OfxImports.Domain.Base;
using OfxImports.Domain.Queries.Response;
using System.IO;

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

                RuleFor(e => e.FileName)
                    .Must(x => Path.GetExtension(x).ToLower() == ".ofx")
                    .WithState(e => EntityError.InvalidFileType);

            }

            public enum EntityError
            {
                InvalidFileName,
                InvalidFileType
            }
        }
    }
}
