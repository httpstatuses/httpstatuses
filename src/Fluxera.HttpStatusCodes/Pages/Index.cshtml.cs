namespace Fluxera.HttpStatusCodes.Pages
{
	using Fluxera.HttpStatusCodes.Model;
	using Fluxera.HttpStatusCodes.Services;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	[ResponseCache(Duration = 60 * 60 * 24, NoStore = false)]
	public class IndexModel : PageModel
	{
		private readonly IStatusCodeModelRepository repository;

		public IndexModel(IStatusCodeModelRepository repository)
		{
			this.repository = repository;
		}

		public IndexPageContent PageContent { get; set; }

		public StatusCodeClasses StatusCodeClasses { get; set; }

		public StatusCodePageContent[] StatusPageContents { get; set; }

		public void OnGet()
		{
			this.PageContent = this.repository.GetIndexPageContent();
			this.StatusCodeClasses = this.repository.GetStatusCodeClasses();
			this.StatusPageContents = this.repository.GetStatusCodePageContents();
		}
	}
}
