﻿using Microsoft.AspNetCore.Mvc;
using OfxImports.Application.Interfaces;
using OfxImports.Domain.Queries.Request;
using OfxImports.Domain.Queries.Response;
using System.Threading.Tasks;
using Tnf.AspNetCore.Mvc.Response;

namespace OfxImports.Web.Controllers
{
    [Route(RouteConsts.OfxImport)]
    public class OfxImportsController : OfxImportsBaseController
    {
        private readonly IOfxImportAppService _ofxImportAppService;

        public OfxImportsController(IOfxImportAppService ofxImportAppService)
        {
            _ofxImportAppService = ofxImportAppService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddOfxImportResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> CreateAsync([FromBody] AddOfxImportCommand command)
        {
            var response = await _ofxImportAppService.AddOfxImport(command);

            return CreateResponseOnPost(response, RouteResponseConsts.OfxImport);
        }
    }
}
