using CACIB.CREW.Api.Core.Service.Simulation;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static CACIB.CREW.Api.Core.Route.ApiRouteConstants;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class GetPiSimulationByDevice : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(SimulationPiRoutes.GetPiSimulationByDevice, async ([FromRoute] long id, IMediator sender) =>
            {
                var result = await sender.Send(new GetPiSimulationByDeviceQuery(id));
                return Results.Ok(result);
            })
            .Produces<GetPiSimulationByDeviceResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetPiSimulationByDevice",
                Summary = "Get PI simulation data by device id",
                Description = "This is an API used to get PI simulation data by given device id"
            })
           .WithName("GetPiSimulationByDevice")
           .WithTags("Simulation");
        }

        public record GetPiSimulationByDeviceQuery(long DeviceId) : IRequest<GetPiSimulationByDeviceResponse>
        {
        }

        public class GetPiSimulationByDeviceHandler(ISimulationPiService service) 
            : IRequestHandler<GetPiSimulationByDeviceQuery, GetPiSimulationByDeviceResponse>
        {
            public async Task<GetPiSimulationByDeviceResponse> Handle(
                GetPiSimulationByDeviceQuery request, CancellationToken cancellationToken)
            {
                return await service.GetPiSimulationByDeviceId(request.DeviceId);
            }
        }
    }
}
