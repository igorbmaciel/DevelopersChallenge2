using Tnf.Application.Services;
using Tnf.Notifications;

namespace OfxImports.Application
{
    public abstract class ApplicationServiceBase : ApplicationService
    {
        protected ApplicationServiceBase(INotificationHandler notification) : base(notification)
        {
        }
    }
}
