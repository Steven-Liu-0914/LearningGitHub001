using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service;
using CACIB.CREW.Api.Core.Service.Common;
using CACIB.CREW.Api.Features.Referential.Model;
using CACIB.CREW.Api.Features.StringResource.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CACIB.CREW.Api.Features.StringResource.Handler;

public class UpdateStringResource
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPut(ApiRouteConstants.StringResourceRoutes.Update, (string type, string key, string cultureCode, [FromBody] StringResourceDto req, IDateTimeHelperService dateTimeHelperService, IMediator sender) =>
            sender.Send(new Request(type, key, cultureCode, req, dateTimeHelperService)));
        }
    }
    public record Request : IRequest<BaseResponse>
    {
        public StringResourceDto Data { get; init; }
        public Request(string type, string key, string cultureCode, StringResourceDto req, IDateTimeHelperService dateTimeHelperService)
        {
            req.ResourceType = type;
            req.ResourceKey = key;
            req.CultureCode = cultureCode;
            req.ModifiedDate = dateTimeHelperService.GetDateTimeNow();
            Data = req;
        }
    }

    public class Handler(IStringResourceService srv) : IRequestHandler<Request, BaseResponse>
    {
        readonly IStringResourceService _srv = srv;

        public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _srv.Update(request.Data);
        }
    }
}