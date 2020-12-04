using OfxImports.Domain.Queries.Request;
using OfxImports.Domain.Queries.Response;
using System.Threading.Tasks;

namespace OfxImports.Application.Interfaces
{
    public interface IOfxImportAppService
    {
        Task<AddOfxImportResponse> AddOfxImport(AddOfxImportCommand command);
    }
}
