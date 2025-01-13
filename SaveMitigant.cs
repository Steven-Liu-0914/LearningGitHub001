using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Mitigants;
using CACIB.CREW.Api.Features.Mitigants.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Mitigants.Handlers
{
    public class SaveMitigant
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPost(ApiRouteConstants.MitigantRoutes.Mitigant, async (MitigantDto mitigant, IMediator sender) =>
                {
                    BaseResponse response = await sender.Send(new SaveMitigantCommand(mitigant));
                    return Results.Ok(response);
                })
                .Produces<BaseResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "SaveMitigant",
                    Summary = "Add update a mitigant along with F1/F2 assignment updates.",
                    Description = "This is an API Used to Save a mitigant in CREW"
                })
                .WithName("SaveMitigant")
                .WithTags("Mitigants")
                .RequireAuthorization();
            }
        }

        public record SaveMitigantCommand(MitigantDto Mitigant) : IRequest<BaseResponse>
        {
        }

        public class Handler(IMitigantService service) : IRequestHandler<SaveMitigantCommand, BaseResponse>
        {
            public async Task<BaseResponse> Handle(SaveMitigantCommand command, CancellationToken cancellationToken)
            {
                return await service.SaveMitigant(command.Mitigant);
            }
        }
    }
}
