using MediatR;
using OfxImports.Application.Interfaces;
using OfxImports.Domain.Queries.Request;
using OfxImports.Domain.Queries.Response;
using System.Threading.Tasks;
using Tnf.Notifications;

namespace OfxImports.Application.Services
{
    public class OfxImportAppService : ApplicationServiceBase, IOfxImportAppService
    {
        private readonly IMediator _mediator;

        public OfxImportAppService(
           INotificationHandler notification,
           IMediator mediator)
           : base(notification)
        {
            _mediator = mediator;
        }

        public async Task<AddOfxImportResponse> AddOfxImport(AddOfxImportCommand command)
        {
            var response = await _mediator.Send(command);

            if (Notification.HasNotification())
                return null;

            return response;
        }
    }
}
