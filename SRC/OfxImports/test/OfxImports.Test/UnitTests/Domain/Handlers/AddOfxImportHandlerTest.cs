using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using OfxImports.Domain.Entities;
using OfxImports.Domain.Factory;
using OfxImports.Domain.Handlers;
using OfxImports.Domain.Interfaces;
using OfxImports.Test.Mocks;
using OfxImports.Tests;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tnf.AspNetCore.TestBase;
using Tnf.Notifications;
using Tnf.Repositories.Uow;
using Xunit;
using static OfxImports.Domain.Queries.Request.AddOfxImportCommand.AddOfxImportValidator;

namespace OfxImports.Test.UnitTests.Domain.Handlers
{
    public class AddOfxImportHandlerTest : TnfAspNetCoreIntegratedTestBase<StartupUnitPropertyTest>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly INotificationHandler _notificationHandler;

        public AddOfxImportHandlerTest()
        {
            _unitOfWorkManager = Substitute.For<IUnitOfWorkManager>();
            _bankAccountRepository = Substitute.For<IBankAccountRepository>();
            _transactionRepository = Substitute.For<ITransactionRepository>();
            _notificationHandler = ServiceProvider.GetRequiredService<INotificationHandler>();
        }

        [Fact]
        public void Shoud_Resolve_All()
        {
            ServiceProvider.GetService<INotificationHandler>().ShouldNotBeNull();
            ServiceProvider.GetService<IBankAccountRepository>().ShouldNotBeNull();
            ServiceProvider.GetService<ITransactionRepository>().ShouldNotBeNull();
            ServiceProvider.GetService<INotificationHandler>().ShouldNotBeNull();
        }

        private AddOfxImportHandler GetOfxImportHandler()
        {
            return new AddOfxImportHandler(
                _notificationHandler,
                _unitOfWorkManager,
                _bankAccountRepository,
                _transactionRepository
                );
        }

        [Fact]
        public async Task Should_Raise_Notification_When_Command_Is_Invalid()
        {
            //parameters
            var command = AddOfxImportCommandMock.GetInvalidDto();

            //call
            var handler = GetOfxImportHandler();

            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            //assert
            Assert.Null(result);
            Assert.True(_notificationHandler.HasNotification());
            Assert.Contains(command.ValidationResult.Errors, e => e.CustomState is EntityError.InvalidFileName);
        }


        [Fact]
        public async Task Should_Raise_Notification_When_Command_FileType_Is_Invalid()
        {
            //parameters
            var command = AddOfxImportCommandMock.GetInvalidDto();

            //call
            var handler = GetOfxImportHandler();

            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            //assert
            Assert.Null(result);
            Assert.True(_notificationHandler.HasNotification());
            Assert.Contains(command.ValidationResult.Errors, e => e.CustomState is EntityError.InvalidFileType);
        }

        [Fact]
        public async Task Should__Raise_Notification_When_File_Not_Exists()
        {
            //parameters
            var command = AddOfxImportCommandMock.GetInvalidFileDto();
          

            //call
            var handler = GetOfxImportHandler();

            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            //assert
            Assert.Null(result);
            Assert.True(_notificationHandler.HasNotification());
            Assert.Contains(_notificationHandler.GetAll(), e => e.DetailedMessage == OfxImportFactory.EntityError.InvalidOfxSouceFile.ToString());
        }

        [Fact]
        public async Task Should_AddOfxImports_With_Already_Exists_BankAccount()
        {
            //parameters
            var command = AddOfxImportCommandMock.GetValidDto();

            var bankAccount = new BankAccount();
            var bankAccountId = Guid.NewGuid();
            var code = 341;
            bankAccount.GetType().GetProperty("Id").SetValue(bankAccount, bankAccountId, null);
            bankAccount.GetType().GetProperty("Code").SetValue(bankAccount, code, null);

            var transaction = new Transaction();
            var transactionId = Guid.NewGuid();
            transaction.GetType().GetProperty("Id").SetValue(transaction, transactionId, null);

            _bankAccountRepository.BankAccountAlreadyExists(bankAccount.Code).ReturnsForAnyArgs(x =>
            {
                return true;
            });

            _bankAccountRepository.GetIdByCode(bankAccount.Code).ReturnsForAnyArgs(x =>
            {
                return bankAccountId;
            });

            _transactionRepository.GetAllTransactions().ReturnsForAnyArgs(x =>
            {
                return new List<Transaction>();
            });          

            //call
            var handler = GetOfxImportHandler();

            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            //assert
            Assert.NotNull(result);
            Assert.False(_notificationHandler.HasNotification());
            Assert.Equal(bankAccount.Code, result.BankAccount.Code);
        }

        [Fact]
        public async Task Should_AddOfxImports_With_Not_Exists_BankAccount()
        {
            //parameters
            var command = AddOfxImportCommandMock.GetValidDto();

            var bankAccount = new BankAccount();
            var bankAccountId = Guid.NewGuid();
            var code = 341;
            bankAccount.GetType().GetProperty("Id").SetValue(bankAccount, bankAccountId, null);
            bankAccount.GetType().GetProperty("Code").SetValue(bankAccount, code, null);

            var transaction = new Transaction();
            var transactionId = Guid.NewGuid();
            transaction.GetType().GetProperty("Id").SetValue(transaction, transactionId, null);

            _bankAccountRepository.BankAccountAlreadyExists(bankAccount.Code).ReturnsForAnyArgs(x =>
            {
                return false;
            });

            _transactionRepository.GetAllTransactions().ReturnsForAnyArgs(x =>
            {
                return new List<Transaction>();
            });

            //call
            var handler = GetOfxImportHandler();

            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            //assert
            Assert.NotNull(result);
            Assert.False(_notificationHandler.HasNotification());
            Assert.Equal(bankAccount.Code, result.BankAccount.Code);
        }

    }
}
