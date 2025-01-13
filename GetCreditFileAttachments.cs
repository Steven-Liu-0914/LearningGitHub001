using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service;
using CACIB.CREW.Api.Features.CreditFile.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.CreditFile.Handlers;

public class GetCreditFileAttachments
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.CreditFileRoutes.GetCreditFileAttachments, async (long? creditFileId, IMediator sender) =>
            {
                GetCreditFileAttachmentsResponse response = await sender.Send(new Query(creditFileId));
                return Results.Ok(response);
            })
            .Produces<GetCreditFileAttachmentsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetCreditFileAttachments",
                Summary = "Get attachments list for given credit_file_id",
                Description = "Get list of attchments with it's CreditFileId, DeviceId, AttachmentType, AttachmentFileName"
            })
            .WithName("GetCreditFileAttachments")
            .WithTags("CreditFile");
        }
    }

    public record Query : IRequest<GetCreditFileAttachmentsResponse>
    {
        public long? CreditFileId { get; init; }

        public Query(long? creditFileId)
        {
            CreditFileId = creditFileId;
        }
    }

    public class Handler(IGetCreditFileDetailsService srv) : IRequestHandler<Query, GetCreditFileAttachmentsResponse>
    {
        private readonly IGetCreditFileDetailsService _srv = srv;

        public async Task<GetCreditFileAttachmentsResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _srv.GetCreditFileAttachments(request.CreditFileId ?? 0);
        }
    }
}
