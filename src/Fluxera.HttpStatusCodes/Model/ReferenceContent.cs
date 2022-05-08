namespace Fluxera.HttpStatusCodes.Model
{
	using Fluxera.Guards;

	public class ReferenceContent
	{
		public ReferenceContent(string text, string example)
		{
			this.Text = Guard.Against.NullOrWhiteSpace(text);
			this.Example = Guard.Against.NullOrWhiteSpace(example);
		}

		public string Text { get; }

		public string Example { get; }
	}
}
