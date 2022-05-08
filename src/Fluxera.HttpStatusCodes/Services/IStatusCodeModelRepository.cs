namespace Fluxera.HttpStatusCodes.Services
{
	using Fluxera.HttpStatusCodes.Model;

	public interface IStatusCodeModelRepository
	{
		StatusCodeClasses GetStatusCodeClasses();

		IndexPageContent GetIndexPageContent();

		StatusCodePageContent GetStatusCodePageContent(int statusCode);

		void Add<T>(string key, T value);
	}
}
