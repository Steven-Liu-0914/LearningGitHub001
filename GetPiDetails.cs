using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Calculation;
using CACIB.CREW.Api.Features.Calculation.Model.PI;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Calculation.Handlers
{
    public class GetPiDetails
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.CalculationRoutes.GetPiDetails, async ([AsParameters] PiDetailRequest request, IMediator sender) =>
                {
                    PiDetailResponse response = await sender.Send(new Query(request));
                    return Results.Ok(response);
                })
                .Produces<PiDetailResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetPiDetails",
                    Summary = "Get PI Details at different levels",
                    Description = "Get PI Details at different levels"
                })
                .WithName("GetPiDetails")
                .WithTags("Calculation");
            }
        }

        public record Query(PiDetailRequest Request) : IRequest<PiDetailResponse>
        {

        }

        public class Handler(ICalculationService calculationService) : IRequestHandler<Query, PiDetailResponse>
        {
            public async Task<PiDetailResponse> Handle(Query query, CancellationToken cancellationToken)
            {
                return await calculationService.GetPiDetailsByLevel(query.Request);
            }
        }
    }
}
