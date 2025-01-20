using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service;
using CACIB.CREW.Api.Features.TextContent.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;
using static CACIB.CREW.Api.Features.TextContent.Handler.GetTextContents;

namespace CACIB.CREW.Api.Features.TextContent.Handler;

public class GetTextContents
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.TextContentRoutes.GetTextContents,
                async ([AsParameters] GetTextContentRequest request, IMediator sender) =>
                {
                    GetTextContentResponse response = await sender.Send(new GetTextContentQuery(request));
                    return Results.Ok(response);
                })
                .Produces<GetTextContentResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetTextContent",
                    Summary = "Get text content In CREW",
                    Description = "Get text content In CREW"
                })
                .WithName("GetTextContents")
                .WithTags("Text Content");
        }
    }

    public record GetTextContentQuery(GetTextContentRequest Request) : IRequest<GetTextContentResponse>
    {
        public string? Scope { get; set; } = Request.Scope;
        public string? Type { get; set; } = Request.Type;
        public string? LanguageCode { get; set; } = Request.LanguageCode;
        public string SortBy { get; set; } = Request.SortBy ?? "Key";
        public bool IsAscending { get; set; } = Request.IsAscending ?? true;
        public int PageIndex { get; init; } = Request.PageIndex ?? 0;
        public int PageSize { get; init; } = Request.PageSize ?? 20;
    }
}

public class Handler(ITextContentService textContentService) : IRequestHandler<GetTextContentQuery, GetTextContentResponse>
{
    private readonly ITextContentService _textContentService = textContentService;
    public async Task<GetTextContentResponse> Handle(GetTextContentQuery query, CancellationToken cancellationToken)
    {
        return await _textContentService.GetTextContentsAsync(query);
    }
}