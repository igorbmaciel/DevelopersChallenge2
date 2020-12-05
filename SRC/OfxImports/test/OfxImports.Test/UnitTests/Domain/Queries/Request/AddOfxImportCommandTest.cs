using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfxImports.Domain;
using OfxImports.Test.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tnf.Notifications;
using Tnf.TestBase;
using Xunit;
using static OfxImports.Domain.Queries.Request.AddOfxImportCommand.AddOfxImportValidator;

namespace OfxImports.Test.UnitTests.Domain.Queries.Request
{
    public class AddOfxImportCommandTest : TnfIntegratedTestBase
    {
        protected override void PreInitialize(IServiceCollection services)
        {
            base.PreInitialize(services);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            services.AddDomainDependency(builder.Build());
        }

        protected override void PostInitialize(IServiceProvider provider)
        {
            base.PostInitialize(provider);

            provider.ConfigureTnf(config =>
            {
                config.UseDomainLocalization();
            });
        }

        [Fact]
        public void Should_Not_Have_Validation_Error_When_Valid_Command()
        {
            var notificationHandler = ServiceProvider.GetRequiredService<INotificationHandler>();
            var command = AddOfxImportCommandMock.GetValidDto();

            //call
            command.IsValid();

            //assert
            Assert.DoesNotContain(command.ValidationResult.Errors, e => e.CustomState is EntityError.InvalidFileName);
            Assert.DoesNotContain(command.ValidationResult.Errors, e => e.CustomState is EntityError.InvalidFileType);
        }

        [Fact]
        public void Should_Have_Validation_Error_When_Invalid_FileName_Command()
        {
            var notificationHandler = ServiceProvider.GetRequiredService<INotificationHandler>();
            var command = AddOfxImportCommandMock.GetInvalidDto();

            //call
            command.IsValid();

            //assert
            Assert.Contains(command.ValidationResult.Errors, e => e.CustomState is EntityError.InvalidFileName);
        }

        [Fact]
        public void Should_Have_Validation_Error_When_Invalid_FileType_Command()
        {
            var notificationHandler = ServiceProvider.GetRequiredService<INotificationHandler>();
            var command = AddOfxImportCommandMock.GetInvalidTypeDto();

            //call
            command.IsValid();

            //assert
            Assert.Contains(command.ValidationResult.Errors, e => e.CustomState is EntityError.InvalidFileType);
        }
    }
}
