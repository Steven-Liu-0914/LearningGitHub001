using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Mitigants;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Mitigants.Handlers
{
    public class DeleteMitigant
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapDelete(ApiRouteConstants.MitigantRoutes.Mitigants, async (int[] mitigantIds, IMediator sender) =>
                {
                    BaseResponse response = await sender.Send(new DeleteMitigantCommand(mitigantIds));
                    return Results.Ok(response);
                })
                .Produces<BaseResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "DeleteMitigant",
                    Summary = "Delete a mitigant",
                    Description = "This is an API Used to Delete a mitigant for given MitigantId in CREW"
                })
                .WithName("DeleteMitigant")
                .WithTags("Mitigants")
                .RequireAuthorization();
            }
        }

        public record DeleteMitigantCommand(int[] Ids) : IRequest<BaseResponse>
        {
        }

        public class Handler(IMitigantService service) : IRequestHandler<DeleteMitigantCommand, BaseResponse>
        {
            public async Task<BaseResponse> Handle(DeleteMitigantCommand command, CancellationToken cancellationToken)
            {
                return await service.DeleteMitigant(command.Ids);
            }
        }
    }
}
