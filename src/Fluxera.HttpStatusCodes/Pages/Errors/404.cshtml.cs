namespace Fluxera.HttpStatusCodes.Pages.Errors
{
	using Fluxera.HttpStatusCodes.Model;
	using Fluxera.HttpStatusCodes.Services;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	public class NotFoundModel : PageModel
	{
		private readonly IStatusCodeModelRepository repository;

		public NotFoundModel(IStatusCodeModelRepository repository)
		{
			this.repository = repository;
		}

		public NotFoundPageContent PageContent { get; set; }

		public void OnGet()
		{
			this.PageContent = this.repository.GetNotFoundPageContent();
		}
	}
}
