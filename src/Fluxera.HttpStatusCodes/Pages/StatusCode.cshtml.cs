namespace Fluxera.HttpStatusCodes.Pages
{
	using Fluxera.HttpStatusCodes.Model;
	using Fluxera.HttpStatusCodes.Services;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	public class StatusCodeModel : PageModel
	{
		private readonly IStatusCodeModelRepository repository;

		public StatusCodeModel(IStatusCodeModelRepository repository)
		{
			this.repository = repository;
		}

		public StatusCodePageContent PageContent { get; set; }

		public StatusCodeClasses StatusCodeClasses { get; set; }

		public void OnGet(int statusCode)
		{
			this.PageContent = this.repository.GetStatusCodePageContent(statusCode);
			this.StatusCodeClasses = this.repository.GetStatusCodeClasses();
		}
	}
}
