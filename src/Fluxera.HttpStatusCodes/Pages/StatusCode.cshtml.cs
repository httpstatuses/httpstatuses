namespace Fluxera.HttpStatusCodes.Pages
{
	using System.Net;
	using System.Reflection;
	using Markdig;
	using Markdig.Extensions.Yaml;
	using Markdig.Syntax;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;
	using SharpYaml.Serialization;

	public class StatusCodeModel : PageModel
	{
		public string StatusCodeMarkdown { get; set; }

		public StatusCodeClasses StatusCodeClasses { get; set; }

		public async Task<IActionResult> OnGetAsync(int statusCode)
		{
			Stream resourceStream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream($"Fluxera.HttpStatusCodes.markdown.codes.{statusCode}.md");

			if(resourceStream != null)
			{
				await using(resourceStream)
				{
					using(StreamReader reader = new StreamReader(resourceStream))
					{
						this.StatusCodeMarkdown = await reader.ReadToEndAsync();

						MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
							.UseYamlFrontMatter()
							.Build();
						MarkdownDocument document = Markdown.Parse(this.StatusCodeMarkdown, pipeline);
						YamlFrontMatterBlock yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();
						string yaml = this.StatusCodeMarkdown.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length);


						Serializer serializer = new Serializer();
						yaml = yaml.Replace("---", "").Trim();
						dynamic obj = serializer.Deserialize(yaml);

						Console.WriteLine(yaml);
					}
				}
			}
			else
			{
				return this.StatusCode((int)HttpStatusCode.NotFound);
			}

			return this.Page();
		}
	}
}
