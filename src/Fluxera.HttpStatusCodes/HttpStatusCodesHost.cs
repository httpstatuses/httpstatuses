namespace Fluxera.HttpStatusCodes
{
	using System;
	using System.IO;
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules.Serilog;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using Serilog;
	using Serilog.Extensions.Hosting;
	using Serilog.Extensions.Logging;

	internal sealed class HttpStatusCodesHost : WebApplicationHost<HttpStatusCodesModule>
	{
		/// <inheritdoc />
		protected override void ConfigureHostBuilder(IHostBuilder builder)
		{
			//// Add OpenTelemetry logging.
			//builder.AddOpenTelemetryLogging(loggerOptions =>
			//{
			//	loggerOptions.AddConsoleExporter();
			//});

			// Add Serilog logging
			builder.AddSerilogLogging((context, options) =>
			{
				options
					.Enrich.WithAssemblyName()
					.Enrich.WithAssemblyVersion()
					.Enrich.WithEnvironmentUserName()
					.Enrich.WithMachineName()
					.Enrich.WithThreadId()
					.Enrich.WithProcessId()
					.Enrich.WithProcessName()
					.Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
					.Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);

				string baseDirectory = AppContext.BaseDirectory;
				string logFilePath = Path.Combine(baseDirectory, "logs", $"{context.HostingEnvironment.ApplicationName}_.log");

				options
					.WriteTo.Async(x => x.File(logFilePath,
						rollingInterval: RollingInterval.Day,
						rollOnFileSizeLimit: true));

				if(context.HostingEnvironment.IsDevelopment())
				{
					options
						.WriteTo.Async(x => x.Console())
						.WriteTo.Async(x => x.Debug());
				}
			});
		}

		/// <inheritdoc />
		protected override ILoggerFactory CreateBootstrapperLoggerFactory(IConfiguration configuration)
		{
			ReloadableLogger bootstrapLogger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.ReadFrom.Configuration(configuration)
				.WriteTo.Console()
				.CreateBootstrapLogger();

			ILoggerFactory loggerFactory = new SerilogLoggerFactory(bootstrapLogger);
			return loggerFactory;
		}
	}
}
