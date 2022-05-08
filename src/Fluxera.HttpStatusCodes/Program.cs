namespace Fluxera.HttpStatusCodes
{
	using System.Reflection;
	using System.Text.Json;
	using Fluxera.Guards;
	using Fluxera.HttpStatusCodes.Model;
	using Fluxera.HttpStatusCodes.Services;
	using Markdig;
	using Markdig.Extensions.Yaml;
	using Markdig.Syntax;
	using SharpYaml.Serialization;
	using Westwind.AspNetCore.Markdown;
	using Markdown = Markdig.Markdown;

	public static class Program
	{
		public static async Task Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddMarkdown(options =>
			{
				options.ConfigureMarkdigPipeline = pipeline =>
				{
					pipeline.UseYamlFrontMatter();
				};
			});
			builder.Services.AddRazorPages();
			builder.Services.AddMemoryCache();
			builder.Services.AddResponseCaching();
			builder.Services.AddSingleton<IStatusCodeModelRepository, StatusCodeModelRepository>();

			WebApplication app = builder.Build();

			// Configure the HTTP request pipeline.
			if(app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/errors/{0}");
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseStaticFiles();

			app.UseResponseCaching();

			app.UseMarkdown();

			app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();

			// Get the memory cache instance, we store our data in.
			IStatusCodeModelRepository repository = app.Services.GetRequiredService<IStatusCodeModelRepository>();

			// Load the status code classes.
			StatusCodeClasses classes = await LoadStatusCodeClassesAsync();
			repository.Add(nameof(StatusCodeClasses), classes);

			// Load the index markdown.
			IndexPageContent indexPageContent = await LoadIndexPageContentAsync();
			repository.Add(nameof(IndexPageContent), indexPageContent);

			// Load the status codes markdown.
			StatusCodePageContent[] statusCodePageContents = await LoadStatusPageContentsAsync();
			foreach(StatusCodePageContent statusCodePageContent in statusCodePageContents)
			{
				repository.Add(StatusCodePageContent.CreateKey(statusCodePageContent.Code), statusCodePageContent);
			}

			await app.RunAsync();
		}

		private static async Task<StatusCodePageContent[]> LoadStatusPageContentsAsync()
		{
			string[] resourceNames = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceNames()
				.Where(x => x.StartsWith("Fluxera.HttpStatusCodes.markdown.codes."))
				.Where(x => x.EndsWith(".md"))
				.ToArray();

			IList<StatusCodePageContent> contents = new List<StatusCodePageContent>(resourceNames.Length);

			foreach(string resourceName in resourceNames)
			{
				Stream resourceStream = Assembly
					.GetExecutingAssembly()
					.GetManifestResourceStream(resourceName);

				if(resourceStream != null)
				{
					await using(resourceStream)
					{
						StatusCodePageContent content = null;

						using(StreamReader reader = new StreamReader(resourceStream))
						{
							string markdown = await reader.ReadToEndAsync();
							IDictionary<string, object> frontMatter = GetFrontMatter(markdown);

							content = new StatusCodePageContent(markdown, frontMatter);
						}

						contents.Add(content);
					}
				}
				else
				{
					throw new InvalidOperationException("The status code page content could not be loaded.");
				}
			}

			return contents.ToArray();
		}

		private static async Task<IndexPageContent> LoadIndexPageContentAsync()
		{
			Stream resourceStream = GetResourceStream("index.md");

			if(resourceStream != null)
			{
				await using(resourceStream)
				{
					IndexPageContent content = null;

					using(StreamReader reader = new StreamReader(resourceStream))
					{
						string markdown = await reader.ReadToEndAsync();
						IDictionary<string, object> frontMatter = GetFrontMatter(markdown);

						content = new IndexPageContent(markdown, frontMatter);
					}

					return content;
				}
			}

			throw new InvalidOperationException("The index page content could not be loaded.");
		}

		private static IDictionary<string, object> GetFrontMatter(string markdown)
		{
			markdown = Guard.Against.NullOrWhiteSpace(markdown);

			MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
				.UseYamlFrontMatter()
				.Build();

			MarkdownDocument document = Markdown.Parse(markdown, pipeline);
			YamlFrontMatterBlock yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();
			string yaml = markdown.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length);

			Serializer serializer = new Serializer();
			yaml = yaml.Replace("---", "").Trim();
			return serializer.Deserialize<IDictionary<string, object>>(yaml);
		}

		private static async Task<StatusCodeClasses> LoadStatusCodeClassesAsync()
		{
			Stream resourceStream = GetResourceStream("classes.json", "codes");

			if(resourceStream != null)
			{
				await using(resourceStream)
				{
					return JsonSerializer.Deserialize<StatusCodeClasses>(resourceStream);
				}
			}

			throw new InvalidOperationException("The status code classes could not be loaded.");
		}

		private static Stream GetResourceStream(string fileName, string folderName = null)
		{
			fileName = Guard.Against.NullOrWhiteSpace(fileName);

			string resourceName = string.IsNullOrWhiteSpace(folderName)
				? $"Fluxera.HttpStatusCodes.markdown.{fileName}"
				: $"Fluxera.HttpStatusCodes.markdown.{folderName}.{fileName}";

			Stream resourceStream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream(resourceName);

			return resourceStream;
		}
	}
}
