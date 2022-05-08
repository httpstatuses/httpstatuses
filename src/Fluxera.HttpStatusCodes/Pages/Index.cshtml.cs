namespace Fluxera.HttpStatusCodes.Pages
{
	using Fluxera.HttpStatusCodes.Model;
	using Fluxera.HttpStatusCodes.Services;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	public class IndexModel : PageModel
	{
		private readonly IStatusCodeModelRepository repository;

		public IndexModel(IStatusCodeModelRepository repository)
		{
			this.repository = repository;
		}

		public IndexPageContent PageContent { get; set; }

		public StatusCodeClasses StatusCodeClasses { get; set; }

		public void OnGet()
		{
			this.PageContent = this.repository.GetIndexPageContent();
			this.StatusCodeClasses = this.repository.GetStatusCodeClasses();
		}
	}
}
