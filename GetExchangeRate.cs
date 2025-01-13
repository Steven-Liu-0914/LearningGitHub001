using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Features.ExchangeRate.Model;
using CACIB.CREW.Api.Features.Referential.HttpClient;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.ExchangeRate.Handler
{
    public class GetExchangeRate
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.ExchangeRateRoutes.GetExchangeRate, async (string? currency, DateTime? date, IMediator sender) =>
                {
                    GetExchangeRateResponse response = await sender.Send(new Query(currency, date));
                    return Results.Ok(response);
                })
                .Produces<GetExchangeRateResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetExchangeRate",
                    Summary = "Get ExchangeRate For date and currency Endpoint",
                    Description = "Returns ExchangeRate for given date and currecny, when date is not provided, current date will be considered. Currency is mandatory",
                })
                .WithName("GetExchangeRate")
                .WithTags("ExchangeRate");
            }
        }

        public record Query : IRequest<GetExchangeRateResponse>
        {
            public Query(string? currency, DateTime? date)
            {
                Currency = currency;
                Date = date;
            }

            public string? Currency { get; set; }
            public DateTime? Date { get; set; }
        }

        public class Handler(IBmaHttpClient httpClient) : IRequestHandler<Query, GetExchangeRateResponse>
        {
            public async Task<GetExchangeRateResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                GetExchangeRateResponse response = new();
                ExchangeRateResult result = await httpClient.GetExchangeRate(request.Currency, request.Date);
                if (result != null)
                {
                    response.Data = result;
                }
                return response;
            }
        }
    }
}
