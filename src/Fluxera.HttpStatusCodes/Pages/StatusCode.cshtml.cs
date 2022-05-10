namespace Fluxera.HttpStatusCodes.Pages
{
	using Fluxera.HttpStatusCodes.Model;
	using Fluxera.HttpStatusCodes.Services;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	[ResponseCache(Duration = 60 * 60 * 24, NoStore = false, VaryByQueryKeys = new string[] { "statusCode" })]
	public class StatusCodeModel : PageModel
	{
		private readonly IStatusCodeModelRepository repository;

		public StatusCodeModel(IStatusCodeModelRepository repository)
		{
			this.repository = repository;
		}

		public StatusCodePageContent PageContent { get; set; }

		public StatusCodeClass StatusCodeClass { get; set; }

		public IActionResult OnGet(int statusCode)
		{
			if(!this.repository.ExistsStatusCodePageContent(statusCode))
			{
				return this.StatusCode(404);
			}

			this.PageContent = this.repository.GetStatusCodePageContent(statusCode);
			this.StatusCodeClass = this.repository.GetStatusCodeClass(this.PageContent.Set);

			return this.Page();
		}
	}
}
