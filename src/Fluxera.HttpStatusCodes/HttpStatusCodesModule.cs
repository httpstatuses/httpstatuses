namespace Fluxera.HttpStatusCodes
{
	using System.Reflection;
	using System.Text.Json;
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Modules.AspNetCore;
	using Fluxera.Extensions.Hosting.Modules.AspNetCore.RazorPages;
	using Fluxera.Extensions.Hosting.Modules.Caching;
	using Fluxera.Guards;
	using Fluxera.HttpStatusCodes.Model;
	using Fluxera.HttpStatusCodes.Services;
	using JetBrains.Annotations;
	using Markdig;
	using Markdig.Extensions.Yaml;
	using Markdig.Syntax;
	using SharpYaml.Serialization;
	using Westwind.AspNetCore.Markdown;

	[UsedImplicitly]
	[DependsOn(typeof(CachingModule))]
	[DependsOn(typeof(RazorPagesModule))]
	internal sealed class HttpStatusCodesModule : ConfigureApplicationModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			context.Log("AddMarkdown", services =>
			{
				services.AddMarkdown();
				services.AddResponseCaching();
				services.AddSingleton<IStatusCodeModelRepository, StatusCodeModelRepository>();
			});
		}

		/// <inheritdoc />
		public override void Configure(IApplicationInitializationContext context)
		{
			WebApplication app = context.GetApplicationBuilder();

			if(app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/errors/{0}");
				app.UseHsts();
			}

			context.UseHttpsRedirection();

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseStaticFiles();

			app.UseResponseCaching();

			app.UseMarkdown();

			context.UseRouting();

			context.UseEndpoints();
		}

		/// <inheritdoc />
		public override void PostConfigure(IApplicationInitializationContext context)
		{
			WebApplication app = context.GetApplicationBuilder();

			// Get the memory cache instance, we store our data in.
			IStatusCodeModelRepository repository = app.Services.GetRequiredService<IStatusCodeModelRepository>();

			// Load the status code classes.
			StatusCodeClasses classes = LoadStatusCodeClasses();
			repository.Add(nameof(StatusCodeClasses), classes);

			// Load the index markdown.
			IndexPageContent indexPageContent = LoadIndexPageContent();
			repository.Add(nameof(IndexPageContent), indexPageContent);

			// Load the status codes markdown.
			StatusCodePageContent[] statusCodePageContents = LoadStatusPageContents();
			foreach(StatusCodePageContent statusCodePageContent in statusCodePageContents)
			{
				repository.Add(StatusCodePageContent.CreateKey(statusCodePageContent.Code), statusCodePageContent);
			}
		}

		private static StatusCodePageContent[] LoadStatusPageContents()
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
					using(resourceStream)
					{
						StatusCodePageContent content = null;

						using(StreamReader reader = new StreamReader(resourceStream))
						{
							string markdown = reader.ReadToEnd();
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

		private static IndexPageContent LoadIndexPageContent()
		{
			Stream resourceStream = GetResourceStream("index.md");

			if(resourceStream != null)
			{
				using(resourceStream)
				{
					IndexPageContent content = null;

					using(StreamReader reader = new StreamReader(resourceStream))
					{
						string markdown = reader.ReadToEnd();
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

			MarkdownDocument document = Markdig.Markdown.Parse(markdown, pipeline);
			YamlFrontMatterBlock yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();
			string yaml = markdown.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length);

			Serializer serializer = new Serializer();
			yaml = yaml.Replace("---", "").Trim();
			return serializer.Deserialize<IDictionary<string, object>>(yaml);
		}

		private static StatusCodeClasses LoadStatusCodeClasses()
		{
			Stream resourceStream = GetResourceStream("classes.json", "codes");

			if(resourceStream != null)
			{
				using(resourceStream)
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
