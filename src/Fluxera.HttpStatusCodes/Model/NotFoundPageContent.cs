namespace Fluxera.HttpStatusCodes.Model
{
	public class NotFoundPageContent : PageContent
	{
		/// <inheritdoc />
		public NotFoundPageContent(string markdown, IDictionary<string, object> frontMatter)
			: base(markdown, frontMatter)
		{
		}

		public string Layout => this.FrontMatter["layout"] as string;
	}
}
