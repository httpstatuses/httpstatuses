namespace Fluxera.HttpStatusCodes.Pages.Errors
{
	using System.Reflection;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	public class InternalServerErrorModel : PageModel
	{
		public string InternalServerErrorMarkdown { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			Stream resourceStream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream("Fluxera.HttpStatusCodes.markdown.error-500.md");

			if(resourceStream != null)
			{
				await using(resourceStream)
				{
					using(StreamReader reader = new StreamReader(resourceStream))
					{
						this.InternalServerErrorMarkdown = await reader.ReadToEndAsync();
					}
				}
			}

			return this.Page();
		}
	}
}
