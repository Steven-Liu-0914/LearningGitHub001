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

public class CreateStringResource
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(ApiRouteConstants.StringResourceRoutes.Create, ([FromBody] StringResourceDto req, IDateTimeHelperService dateTimeHelperService, IMediator sender) =>
            sender.Send(new Request(req, dateTimeHelperService)));
        }
    }
    public record Request : IRequest<BaseResponse>
    {
        public StringResourceDto Data { get; init; }
        public Request(StringResourceDto req, IDateTimeHelperService dateTimeHelperService)
        {
            req.CreatedDate = dateTimeHelperService.GetDateTimeNow();
            //TODO: replaced by user authentication
            req.CreatedBy = "TestUser";
            Data = req;
        }
    }

    public class Handler(IStringResourceService srv) : IRequestHandler<Request, BaseResponse>
    {
        readonly IStringResourceService _srv = srv;

        public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _srv.Add(request.Data);
        }
    }
}
