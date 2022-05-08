namespace Fluxera.HttpStatusCodes.Pages.Errors
{
	using System.Reflection;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	public class NotFoundModel : PageModel
	{
		public string NotFoundMarkdown { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			Stream resourceStream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream("Fluxera.HttpStatusCodes.markdown.error-404.md");

			if(resourceStream != null)
			{
				await using(resourceStream)
				{
					using(StreamReader reader = new StreamReader(resourceStream))
					{
						this.NotFoundMarkdown = await reader.ReadToEndAsync();
					}
				}
			}

			return this.Page();
		}
	}
}
