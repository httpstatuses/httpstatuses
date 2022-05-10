namespace Fluxera.HttpStatusCodes
{
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Modules.AspNetCore;
	using Fluxera.Extensions.Hosting.Modules.AspNetCore.Cors;
	using Fluxera.Extensions.Hosting.Modules.AspNetCore.RazorPages;
	using Fluxera.Extensions.Hosting.Modules.Caching;
	using Fluxera.HttpStatusCodes.Contributors;
	using Fluxera.HttpStatusCodes.Services;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Westwind.AspNetCore.Markdown;

	[UsedImplicitly]
	[DependsOn(typeof(CachingModule))]
	[DependsOn(typeof(RazorPagesModule))]
	[DependsOn(typeof(CorsModule))]
	internal sealed class HttpStatusCodesModule : ConfigureApplicationModule
	{
		/// <inheritdoc />
		public override void PreConfigureServices(IServiceConfigurationContext context)
		{
			// Add the endpoint route contributor.
			context.Log("AddEndpointRouteContributor",
				services => services.AddEndpointRouteContributor<EndpointRouteContributor>());
		}

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

			if(context.Environment.IsDevelopment())
			{
				context.UseDeveloperExceptionPage();
			}
			else
			{
				context.UseExceptionHandler("/errors/500");
				app.UseHsts();
			}

			context.UseHttpsRedirection();

			context.UseStatusCodePagesWithReExecute("/errors/{0}");

			context.UseStaticFiles();

			context.UseResponseCaching();

			context.UseMarkdown();

			context.UseRouting();

			context.UseCors();

			context.UseEndpoints();
		}

		/// <inheritdoc />
		public override void PostConfigure(IApplicationInitializationContext context)
		{
			context.LoadStatusCodeData();
		}
	}
}
