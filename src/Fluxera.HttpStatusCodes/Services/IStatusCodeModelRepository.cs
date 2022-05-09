namespace Fluxera.HttpStatusCodes.Services
{
	using Fluxera.HttpStatusCodes.Model;

	public interface IStatusCodeModelRepository
	{
		StatusCodeClasses GetStatusCodeClasses();

		StatusCodeClass GetStatusCodeClass(int statusCodeClass);

		IndexPageContent GetIndexPageContent();

		NotFoundPageContent GetNotFoundPageContent();

		StatusCodePageContent[] GetStatusCodePageContents();

		StatusCodePageContent GetStatusCodePageContent(int statusCode);

		bool ExistsStatusCodePageContent(int statusCode);

		void Add<T>(string key, T value);
	}
}
