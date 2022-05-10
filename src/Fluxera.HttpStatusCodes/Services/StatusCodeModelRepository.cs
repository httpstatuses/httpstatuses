namespace Fluxera.HttpStatusCodes.Services
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
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
		public StatusCodeClass GetStatusCodeClass(int statusCodeClass)
		{
			if(this.GetStatusCodeClasses().TryGetValue(statusCodeClass, out StatusCodeClass value))
			{
				return value;
			}

			throw new InvalidOperationException($"The status code class '{statusCodeClass}' could not be loaded.");
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
		public NotFoundPageContent GetNotFoundPageContent()
		{
			if(this.store.TryGetValue(nameof(NotFoundPageContent), out object value))
			{
				return (NotFoundPageContent)value;
			}

			throw new InvalidOperationException("The not found page content could not be loaded.");
		}

		/// <inheritdoc />
		public StatusCodePageContent[] GetStatusCodePageContents()
		{
			if(this.store.TryGetValue(nameof(StatusCodePageContent), out object value))
			{
				return (StatusCodePageContent[])value;
			}

			throw new InvalidOperationException("The status page contents could not be loaded.");
		}

		/// <inheritdoc />
		public StatusCodePageContent GetStatusCodePageContent(int statusCode)
		{
			StatusCodePageContent value = this.GetStatusCodePageContents().FirstOrDefault(x => x.Code == statusCode);
			if(value != null)
			{
				return value;
			}

			throw new InvalidOperationException($"The status page content '{statusCode}' could not be loaded.");
		}

		/// <inheritdoc />
		public bool ExistsStatusCodePageContent(int statusCode)
		{
			return this.GetStatusCodePageContents().Any(x => x.Code == statusCode);
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
