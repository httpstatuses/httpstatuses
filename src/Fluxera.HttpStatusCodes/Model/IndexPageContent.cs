namespace Fluxera.HttpStatusCodes.Model
{
	using System.Collections.Generic;

	public class IndexPageContent : PageContent
	{
		/// <inheritdoc />
		public IndexPageContent(string markdown, IDictionary<string, object> frontMatter)
			: base(markdown, frontMatter)
		{
		}

		public string Layout => this.FrontMatter["layout"] as string;
	}
}
