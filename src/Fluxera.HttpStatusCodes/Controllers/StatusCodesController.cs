namespace Fluxera.HttpStatusCodes.Controllers
{
	using System;
	using System.Linq;
	using Fluxera.HttpStatusCodes.Model;
	using Fluxera.HttpStatusCodes.Services;
	using Microsoft.AspNetCore.Cors;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.WebUtilities;

	[ApiController]
	public sealed class StatusCodesController : ControllerBase
	{
		private readonly IStatusCodeModelRepository repository;

		public StatusCodesController(IStatusCodeModelRepository repository)
		{
			this.repository = repository;
		}

		[HttpGet("{statusCode}.json")]
		[EnableCors("Default")]
		[Produces("application/json", Type = typeof(ProblemDetails))]
		[ResponseCache(Duration = 60 * 60 * 24, NoStore = false, VaryByQueryKeys = new string[] { "statusCode" })]
		public IActionResult Get(string statusCode)
		{
			try
			{
				int.TryParse(statusCode, out int httpStatusCode);

				if(!this.repository.ExistsStatusCodePageContent(httpStatusCode))
				{
					return this.Problem(
						statusCode: 404,
						type: "https://httpstatuscodes.io/404",
						title: ReasonPhrases.GetReasonPhrase(404),
						instance: $"https://httpstatuscodes.io/{statusCode}.json");
				}

				StatusCodePageContent content = this.repository.GetStatusCodePageContent(httpStatusCode);
				StatusCodeClass statusCodeClass = this.repository.GetStatusCodeClass(content.Set);

				return this.Ok(new
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
				return this.Problem(
					statusCode: 500,
					type: "https://httpstatuscodes.io/500",
					title: ReasonPhrases.GetReasonPhrase(500),
					instance: $"https://httpstatuscodes.io/{statusCode}.json");
			}
		}
	}
}
