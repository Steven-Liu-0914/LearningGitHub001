using CACIB.CREW.Api.Core.Common;
using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Simulation;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using CACIB.CREW.Api.Utility;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class CreatePiSimulation
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPost(ApiRouteConstants.SimulationPiRoutes.CreatePiSimulation, async (PiSimulationRequest request, HttpContext context, IMediator sender) =>
                {
                    BaseResponse response = await sender.Send(new CreateSimulationPiCommand(request));
                    return Results.Ok(response);
                })
                .Produces<BaseResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "CreatePiSimulation",
                    Summary = "Create a new credilt file",
                    Description = "Create a new credilt file for simulation pi"
                })
                .WithName("Create PI Simulation")
                .WithTags("Simulation")
                .RequireAuthorization();
            }
        }

        public record CreateSimulationPiCommand(PiSimulationRequest request) : IRequest<BaseResponse>
        {
            public string Name { get; init; } = request.Name;
            public string? Description { get; init; } = request.Description;
            public decimal Amount { get; init; } = request.Amount;
            public string Currency { get; init; } = request.Currency;
            public decimal ExchangeRate { get; init; } = request.ExchangeRate;
            public int? BeneficiaryKorusId { get; set; } = request.BeneficiaryKorusId;
            public int? BeneficiaryId { get; set; } = request.BeneficiaryId;
            public string? BeneficiaryLongName { get; set; } = request.BeneficiaryLongName;
            public int? BeneficiaryKycId { get; set; } = request.BeneficiaryKycId;
            public string BeneficiaryType { get; set; } = request.BeneficiaryType;
            public string SimulationType { get; set; } = SimulationTypeCodes.PI;

            //If BeneficiaryKorusId has value and BeneficiaryKycId is null => Beneficiary Import from Id CIB
            //If BeneficiaryKycId has value and BeneficiaryKorusId is null => Beneficiary Import from Kyc
            //Else => Manaul Created Beneficiary
            public string BeneficiarySource { get; set; } = (request.BeneficiaryKorusId != null && request.BeneficiaryKorusId > 0 && request.BeneficiaryKycId == null) ? CpyAddRef.Ricos :
                ((request.BeneficiaryKycId != null && request.BeneficiaryKycId > 0 && request.BeneficiaryKorusId == null ) ? CpyAddRef.Kyc : CpyAddRef.Manual);

        }

        public class Handler(ISimulationPiService simulationPiService) : IRequestHandler<CreateSimulationPiCommand, BaseResponse>
        {
            private readonly ISimulationPiService _simulationPiService = simulationPiService;

            public async Task<BaseResponse> Handle(CreateSimulationPiCommand request, CancellationToken cancellationToken)
            {
                return await _simulationPiService.CreateSimulation(request);
            }
        }
    }
}
