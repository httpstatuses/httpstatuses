namespace Fluxera.HttpStatusCodes
{
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting;

	public static class Program
	{
		public static async Task Main(string[] args)
		{
			await ApplicationHost.RunAsync<HttpStatusCodesHost>(args);
		}
	}
}
