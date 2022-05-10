namespace Fluxera.HttpStatusCodes.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class StatusCodePageContent : PageContent
	{
		/// <inheritdoc />
		public StatusCodePageContent(string markdown, IDictionary<string, object> frontMatter)
			: base(markdown, frontMatter)
		{
			if(frontMatter.TryGetValue("references", out object value))
			{
				IDictionary<object, object> dict = (IDictionary<object, object>)value;

				IList<ReferenceContent> references = new List<ReferenceContent>(dict.Count);
				foreach((object text, object example) in dict)
				{
					references.Add(new ReferenceContent(text as string, example as string));
				}

				this.References = references.ToArray();
			}
			else
			{
				this.References = Array.Empty<ReferenceContent>();
			}
		}

		public int Set => (int)this.FrontMatter["set"];

		public int Code => (int)this.FrontMatter["code"];

		public string Excerpt => (string)this.FrontMatter["excerpt"];

		public bool IsUnlisted => this.FrontMatter.ContainsKey("unlisted") && (bool)this.FrontMatter["unlisted"];

		public ReferenceContent[] References { get; }
	}
}
