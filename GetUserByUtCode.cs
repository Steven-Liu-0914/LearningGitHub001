using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.UserAccess;
using CACIB.CREW.Api.Features.UserAccess.Model.DTO.ResponseDto;
using CACIB.CREW.Api.Features.UserAccess.Model.Response;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.UserAccess.Handlers;

public class GetUserByUtCode
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.UserAccessRoutes.GetUserByUtCode, async (string utCode, IMediator sender) =>
            {
                GetUserByUtCodeResponse response = new()
                {
                    Data = await sender.Send(new Query(utCode))
                };

                return Results.Ok(response);
            })
            .Produces<UserResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetUserByUtCode",
                Summary = "Get User Object By UtCode Endpoint",
                Description = "Get User Object By UtCode Endpoint"
            })
            .WithName("GetUserByUtCode")
            .WithTags("UserAccess");
        }
    }

    public record Query : IRequest<UserResponseDto>
    {
        public string UtCode { get; init; }
        public Query(string utCode)
        {
            UtCode = utCode;
        }
    }

    public class Handler(IUserAccessService srv) : IRequestHandler<Query, UserResponseDto>
    {
        private readonly IUserAccessService _srv = srv;

        public async Task<UserResponseDto> Handle(Query request, CancellationToken cancellationToken)
        {
            UserResponseDto response = await _srv.GetUserByUtCode(request.UtCode);

            return response;
        }
    }

}