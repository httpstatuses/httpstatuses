namespace Fluxera.HttpStatusCodes.Pages.Errors
{
	using System.IO;
	using System.Reflection;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	[IgnoreAntiforgeryToken]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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
