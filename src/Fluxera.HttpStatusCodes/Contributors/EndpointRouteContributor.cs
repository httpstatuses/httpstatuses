namespace Fluxera.HttpStatusCodes.Contributors
{
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules.AspNetCore;
	using Fluxera.HttpStatusCodes.Model;
	using Fluxera.HttpStatusCodes.Services;
	using Microsoft.AspNetCore.WebUtilities;

	internal sealed class EndpointRouteContributor : IEndpointRouteContributor
	{
		/// <inheritdoc />
		public void MapRoute(IEndpointRouteBuilder routeBuilder, IApplicationInitializationContext context)
		{
			context.Log("MapJsonEndpoint", _ =>
			{
				// Add a JSON endpoint.
				routeBuilder
					.MapGet("/{statusCode}.json", (string statusCode, IStatusCodeModelRepository repository) =>
					{
						try
						{
							int.TryParse(statusCode, out int httpStatusCode);

							if(!repository.ExistsStatusCodePageContent(httpStatusCode))
							{
								return Results.Problem(
									statusCode: 404,
									type: "https://httpstatuscodes.io/404",
									title: ReasonPhrases.GetReasonPhrase(404),
									instance: $"https://httpstatuscodes.io/{statusCode}.json");
							}

							StatusCodePageContent content = repository.GetStatusCodePageContent(httpStatusCode);
							StatusCodeClass statusCodeClass = repository.GetStatusCodeClass(content.Set);

							return Results.Ok(new
							{
								location = $"https://httpstatuscodes.io/{httpStatusCode}",
								statusCode = content.Code,
								title = content.Title,
								category = statusCodeClass.Title
									.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
									.LastOrDefault() ?? string.Empty,
								description = content.Excerpt
							});
						}
						catch
						{
							return Results.Problem(
								statusCode: 500,
								type: "https://httpstatuscodes.io/500",
								title: ReasonPhrases.GetReasonPhrase(500),
								instance: $"https://httpstatuscodes.io/{statusCode}.json");
						}
					})
					.Produces(200, contentType: "application/json")
					.ProducesProblem(200, "application/json")
					.RequireCors("Default");
			});
		}
	}
}
