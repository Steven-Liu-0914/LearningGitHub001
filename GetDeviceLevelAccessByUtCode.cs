using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.UserAccess;
using CACIB.CREW.Api.Features.UserAccess.Model.DTO;
using CACIB.CREW.Api.Features.UserAccess.Model.Response;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.UserAccess.Handlers;

public class GetDeviceLevelAccessByUtCode
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.UserAccessRoutes.GetDeviceLevelAccessByUtCode, async (string utCode, IMediator sender) =>
            {
                GetDeviceLevelAccessByUtCodeResponse response = new()
                {
                    Data = (await sender.Send(new Query(utCode))).ToList()
                };

                await sender.Send(new Query(utCode));
                return Results.Ok(response);
            })
            .Produces<List<DeviceLevelAccessDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetDeviceLevelAccessByUtCode",
                Summary = "Get Device Level Access By UtCode Endpoint",
                Description = "Get Device Level Access By UtCode Endpoint"
            })
            .WithName("GetDeviceLevelAccessByUtCode")
            .WithTags("UserAccess");
        }
    }

    public record Query : IRequest<IEnumerable<DeviceLevelAccessDto>>
    {
        public string UtCode { get; init; }

        public Query(string utCode)
        {
            UtCode = utCode;
        }
    }

    public class Handler(IUserAccessService srv) : IRequestHandler<Query, IEnumerable<DeviceLevelAccessDto>>
    {
        private readonly IUserAccessService _srv = srv;

        public async Task<IEnumerable<DeviceLevelAccessDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            IEnumerable<DeviceLevelAccessDto> deviceLevelAccesses = await _srv.GetDeviceLevelRoles(request.UtCode);

            return deviceLevelAccesses;
        }
    }
}