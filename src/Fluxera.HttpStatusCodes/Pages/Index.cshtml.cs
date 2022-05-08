namespace Fluxera.HttpStatusCodes.Pages
{
	using System.Reflection;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	public class IndexModel : PageModel
	{
		public string IndexMarkdown { get; set; }

		public StatusCodeClasses StatusCodeClasses { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			Stream resourceStream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream("Fluxera.HttpStatusCodes.markdown.index.md");

			if(resourceStream != null)
			{
				await using(resourceStream)
				{
					using(StreamReader reader = new StreamReader(resourceStream))
					{
						this.IndexMarkdown = await reader.ReadToEndAsync();
					}
				}
			}

			resourceStream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream("Fluxera.HttpStatusCodes.markdown.codes.classes.json");

			if(resourceStream != null)
			{
				await using(resourceStream)
				{
					this.StatusCodeClasses = JsonSerializer.Deserialize<StatusCodeClasses>(resourceStream);
				}
			}

			return this.Page();
		}
	}

	public class StatusCodeClasses : Dictionary<string, StatusCodeClass>
	{
	}

	public class StatusCodeClass
	{
		[JsonPropertyName("title")]
		public string Title { get; set; }
	}
}
