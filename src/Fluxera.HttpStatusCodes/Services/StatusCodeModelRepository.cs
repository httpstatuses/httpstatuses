namespace Fluxera.HttpStatusCodes.Services
{
	using System.Collections.Concurrent;
	using Fluxera.HttpStatusCodes.Model;

	public sealed class StatusCodeModelRepository : IStatusCodeModelRepository
	{
		private readonly ConcurrentDictionary<string, object> store = new ConcurrentDictionary<string, object>();

		/// <inheritdoc />
		public StatusCodeClasses GetStatusCodeClasses()
		{
			if(this.store.TryGetValue(nameof(StatusCodeClasses), out object value))
			{
				return (StatusCodeClasses)value;
			}

			throw new InvalidOperationException("The status code classes could not be loaded.");
		}

		/// <inheritdoc />
		public IndexPageContent GetIndexPageContent()
		{
			if(this.store.TryGetValue(nameof(IndexPageContent), out object value))
			{
				return (IndexPageContent)value;
			}

			throw new InvalidOperationException("The index page content could not be loaded.");
		}

		/// <inheritdoc />
		public StatusCodePageContent GetStatusCodePageContent(int statusCode)
		{
			if(this.store.TryGetValue(StatusCodePageContent.CreateKey(statusCode), out object value))
			{
				return (StatusCodePageContent)value;
			}

			throw new InvalidOperationException($"The status page content ('{statusCode}') could not be loaded.");
		}

		/// <inheritdoc />
		public void Add<T>(string key, T value)
		{
			if(!this.store.TryAdd(key, value))
			{
				throw new InvalidOperationException($"The data could not be added for key '{key}'.");
			}
		}
	}
}
