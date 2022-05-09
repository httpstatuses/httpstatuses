namespace Fluxera.HttpStatusCodes
{
	using Fluxera.Extensions.Hosting;

	internal sealed class HttpStatusCodesHost : WebApplicationHost<HttpStatusCodesModule>
	{
		///// <inheritdoc />
		//protected override void ConfigureHostBuilder(IHostBuilder builder)
		//{
		//	// Add OpenTelemetry logging.
		//	builder.AddOpenTelemetryLogging(loggerOptions =>
		//	{
		//		loggerOptions.AddConsoleExporter();
		//	});

		//	// Add Serilog logging
		//	builder.AddSerilogLogging(loggerOptions =>
		//	{
		//		loggerOptions
		//			.MinimumLevel.Information()
		//			.WriteTo.Console();
		//	});
		//}

		///// <inheritdoc />
		//protected override ILoggerFactory CreateBootstrapperLoggerFactory(IConfiguration configuration)
		//{
		//	ReloadableLogger bootstrapLogger = new LoggerConfiguration()
		//		.Enrich.FromLogContext()
		//		.ReadFrom.Configuration(configuration)
		//		.WriteTo.Console()
		//		.CreateBootstrapLogger();

		//	ILoggerFactory loggerFactory = new SerilogLoggerFactory(bootstrapLogger);
		//	return loggerFactory;
		//}
	}
}
